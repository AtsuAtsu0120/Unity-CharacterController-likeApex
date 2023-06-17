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
    public InputAction moveAction;
    public InputAction lookAction;

    public InputAction upAction;
    public InputAction downAction;
    public InputAction sprintAction;

    #endregion

    #region Fields_Other

    public Rigidbody rb;

    private Transform camTransform;
    private float3 inputVector;

    private float crouchCooltime;
    private float standupCameraHeghit;

    private State currentState;

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

        currentState = new WalkOnGround(this);
    }

    // Update is called once per frame
    private void Update()
    {
        currentState?.OnUpdate();
    }
    public void ChangeState<T>(T state)where T : State
    {
        currentState?.OnExit();
        currentState = state;
        currentState.OnEnter();
    }
}
