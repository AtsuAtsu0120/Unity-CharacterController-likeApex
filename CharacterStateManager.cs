using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterStateManager : MonoBehaviour
{
    #region Fields_FromInspector

    [Header("Value Settings")]
    [SerializeField] private float Speed = 50.0f;
    public float JumpPower = 1000.0f;
    public float Sensibility = 1.0f;
    [SerializeField] private float CrounchSpeed = 25.0f;
    [SerializeField] private float CrounchBoost = 3.0f;
    [SerializeField] private float CrouchCameraHeghit = 0.2f;

    [SerializeField] private float MaxSpeed = 8.0f;
    [SerializeField] private float MaxCrouchSpeed = 15.0f;
    [SerializeField] private float MaxSprintSpeed = 13.0f;
    [SerializeField] private float MaxClimbSpeed = 5.0f;

    public float MaxLimit = 85;
    [HideInInspector] public float MinLimit;


    [Header("UI")]
    [SerializeField] private TextMeshProUGUI text;
    [Header("Layer")]
    [SerializeField] private LayerMask WallLayer;

    #endregion

    #region Fields_InputSystem

    private StelthInputActions actions;
    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction lookAction;

    [HideInInspector] public InputAction upAction;
    [HideInInspector] public InputAction downAction;
    [HideInInspector] public InputAction sprintAction;

    #endregion

    #region Fields_Other

    public Rigidbody rb;

    public Transform camTransform;
    private float3 inputVector;

    private float crouchCooltime;
    private float standupCameraHeghit;

    private bool isWallMode = false;

    private bool onGround = false;
    private bool onWall = false;

    private bool isCrouch = false;
    private bool isSprintBeforeCrouch;
    private bool isSprint = false;
    private bool isJumping = false;

    private bool isStraight = false;

    private bool fixedHorizontalLook = false;

    #endregion

    #region Fields_State

    private MovementState currentMovementState;
    private ViewpointState currentViewpointState;

    #endregion

    void Start()
    {
        //カーソルの無効化
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //InputSystemの初期化
        actions = new();
        actions.Enable();
        moveAction = actions.Player.Move;
        lookAction = actions.Player.Look;

        upAction = actions.Player.Up;
        downAction = actions.Player.Down;
        sprintAction = actions.Player.Sprint;

        //ここまで

        //Rigidbodyの取得
        rb = GetComponent<Rigidbody>();
        //カメラの場所を取得
        camTransform = transform.GetChild(0).transform;

        MinLimit = 360 - MaxLimit;

        ChangeState(new AirWalk(this));

        currentViewpointState = new NormalViewpoint(this);
    }

    // Update is called once per frame
    private void Update()
    {
        currentMovementState?.OnUpdate();
        currentViewpointState?.OnUpdate();
    }
    private void FixedUpdate()
    {
        currentMovementState?.OnFixedUpdate();
    }
    private void OnCollisionEnter(Collision collision)
    {
        currentMovementState.OnEnterCollider(collision);
    }
    public void ChangeState(MovementState state)
    {
        upAction.Disable();
        downAction.Disable();
        sprintAction.Disable();

        ////キーをリセット
        if (currentMovementState is not null)
        {
            upAction.performed -= currentMovementState.OnPerformUp;
            downAction.performed -= currentMovementState.OnPerformDown;
            downAction.canceled -= currentMovementState.OnCancelDown;
            sprintAction.performed -= currentMovementState.OnPerformSprint;
            sprintAction.canceled -= currentMovementState.OnCancelSprint;
        }

        currentMovementState?.OnExit();
        currentMovementState = state;

        ////キーを設定
        upAction.performed += currentMovementState.OnPerformUp;
        downAction.performed += currentMovementState.OnPerformDown;
        downAction.canceled += currentMovementState.OnCancelDown;
        sprintAction.performed += currentMovementState.OnPerformSprint;
        sprintAction.canceled += currentMovementState.OnCancelSprint;

        upAction.Enable();
        downAction.Enable();
        sprintAction.Enable();

        currentMovementState.OnEnter();
    }
}
