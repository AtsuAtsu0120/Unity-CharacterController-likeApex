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
    [SerializeField] private float JumpPower = 1000.0f;
    [SerializeField] private float Sensibility = 1.0f;
    [SerializeField] private float CrounchSpeed = 25.0f;
    [SerializeField] private float CrounchBoost = 3.0f;
    [SerializeField] private float CrouchCameraHeghit = 0.2f;

    [SerializeField] private float MaxSpeed = 8.0f;
    [SerializeField] private float MaxCrouchSpeed = 15.0f;
    [SerializeField] private float MaxSprintSpeed = 13.0f;
    [SerializeField] private float MaxClimbSpeed = 5.0f;

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

    private Transform camTransform;
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
    #endregion

    void Start()
    {
        //ÉJÅ[É\ÉãÇÃñ≥å¯âª
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //InputSystemÇÃèâä˙âª
        actions = new();
        actions.Enable();
        moveAction = actions.Player.Move;
        lookAction = actions.Player.Look;
        //Ç±Ç±Ç‹Ç≈

        //RigidbodyÇÃéÊìæ
        rb = GetComponent<Rigidbody>();

        currentMovementState = new WalkOnGround(this);
        currentMovementState.OnEnter();
    }

    // Update is called once per frame
    private void Update()
    {
        currentMovementState?.OnUpdate();
    }
    private void FixedUpdate()
    {
        currentMovementState?.OnFixedUpdate();
    }
    public void ChangeState<T>(T state)where T : MovementState
    {
        currentMovementState?.OnExit();
        currentMovementState = state;
        currentMovementState.OnEnter();
    }
}
