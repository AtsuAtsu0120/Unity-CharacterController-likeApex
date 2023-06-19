using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterStateManager : MonoBehaviour
{
    #region Fields_FromInspector

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
    public float3 inputVector;

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

        upAction.Enable();
        downAction.Enable();
        sprintAction.Enable();

        //ここまで

        //Rigidbodyの取得
        rb = GetComponent<Rigidbody>();

        //カメラの場所を取得
        camTransform = transform.GetChild(0).transform;

        ChangeState(new AirWalk(this));

        currentViewpointState = new NormalViewpoint(this);
    }

    // Update is called once per frame
    private void Update()
    {
        currentMovementState?.OnUpdate();
        currentViewpointState?.OnUpdate();

        Debug.Log(currentMovementState.GetType());
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

        //キーを設定
        upAction.performed += currentMovementState.OnPerformUp;
        downAction.performed += currentMovementState.OnPerformDown;
        downAction.canceled += currentMovementState.OnCancelDown;
        sprintAction.performed += currentMovementState.OnPerformSprint;
        sprintAction.canceled += currentMovementState.OnCancelSprint;

        currentMovementState.OnEnter();
    }
    public void ChangeViewPointState(ViewpointState state)
    {
        currentViewpointState?.OnExit();
        currentViewpointState = state;
        currentViewpointState.OnEnter();
    }
}
