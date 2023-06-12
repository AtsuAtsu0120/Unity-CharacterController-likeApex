using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    #region Fields_FromInspector

    [Header("Value Settings")]
    [SerializeField] private float Speed = 100.0f;
    [SerializeField] private float JumpPower = 1000.0f;
    [SerializeField] private float Sensibility = 1.0f;
    [SerializeField] private float CrounchSpeed = 50.0f;
    [SerializeField] private float CrounchBoost = 3.0f;
    [SerializeField] private float CrouchCameraHeghit = 0.2f;

    [SerializeField] private float MaxSpeed = 10.0f;
    [SerializeField] private float MaxCrouchSpeed = 20.0f;
    [SerializeField] private float MaxSprintSpeed = 12.0f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI text;

    #endregion

    #region Fields_InputSystem

    private StelthInputActions actions;
    private InputAction moveAction;
    private InputAction lookAction;

    private InputAction upAction;
    private InputAction downAction;
    private InputAction sprintAction;

    #endregion

    #region Fields_Other

    private Rigidbody rb;
    private Transform camTransform;
    private float3 inputVector;

    private float crouchCooltime;
    private float standupCameraHeghit;

    private bool onGround = false;

    private bool isCrouch = false;
    private bool isSprintBeforeCrouch;
    private bool isSprint = false;

    #endregion
    #region Fields_Constant

    private readonly float MaxLimit = 85;
    private readonly float MinLimit = 360 - 85;
    private float DefaultMaxCrouchSpeed;

    #endregion

    #region Methods_Unity
    private void Start()
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
        upAction.performed += PerformUpAction;

        downAction = actions.Player.Down;
        downAction.performed += PerformDownAction;
        downAction.canceled += CancelDownAction;

        sprintAction = actions.Player.Sprint;
        sprintAction.performed += PerformSprintAction;
        sprintAction.canceled += CancelSpritAction;
        //ここまで

        //Rigidbodyの取得
        rb = GetComponent<Rigidbody>();

        //子のカメラを取得
        camTransform = transform.GetChild(0).transform;
        standupCameraHeghit = camTransform.localPosition.y;

        //しゃがんでいるときの速度を保存しておく
        DefaultMaxCrouchSpeed = MaxCrouchSpeed;
    }
    private void OnDisable()
    {
        actions.Disable();
        actions = null;
    }
    private void Update()
    {
        //視点移動
        var lookVector = lookAction.ReadValue<Vector2>();

        var camLocalAngle = transform.localEulerAngles;
        camLocalAngle.x += -lookVector.y * Sensibility;
        if (camLocalAngle.x > MaxLimit && camLocalAngle.x < 180)
        {
            camLocalAngle.x = MaxLimit;
        }
        if (camLocalAngle.x < MinLimit && camLocalAngle.x > 180)
        {
            camLocalAngle.x = MinLimit;
        }
        camLocalAngle.y += lookVector.x * Sensibility;
        transform.localEulerAngles = camLocalAngle;

        //移動
        float2 movementVector = moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        inputVector.x = movementVector.x;
        inputVector.z = movementVector.y;
    }
    private void FixedUpdate()
    {
        #region CharactorMovement

        //重力
        if (!onGround)
        {
            inputVector.y = Physics.gravity.y;
        }

        //スライディングのクールタイムの減少
        if (crouchCooltime >= 0.0f)
        {
            crouchCooltime -= Time.deltaTime;
        }

        AddForceForward(rb, transform, inputVector);

        #endregion

        #region CrounchCamera

        //しゃがみのカメラ移動。
        if (isCrouch && camTransform.localPosition.y > CrouchCameraHeghit)
        {
            var position = camTransform.transform.localPosition;
            position.y -= 0.01f;
            camTransform.localPosition = position;
        }
        else if(!isCrouch && camTransform.localPosition.y < standupCameraHeghit)
        {
            var position = camTransform.transform.localPosition;
            position.y += 0.01f;
            camTransform.localPosition = position;
        }

        #endregion
    }
    #endregion

    #region Methods_Action

    private void PerformUpAction(InputAction.CallbackContext ctx)
    {
        if (!onGround) return;
        rb.AddForce(new float3(0, JumpPower, 0));
    }
    private void PerformDownAction(InputAction.CallbackContext ctx)
    {
        isCrouch = true;
        isSprintBeforeCrouch = isSprint;
        isSprint = false;
        if (crouchCooltime <= 0.0f)
        {
            inputVector.z += CrounchBoost;
            crouchCooltime = 5.0f;
        }
    }
    private void CancelDownAction(InputAction.CallbackContext ctx)
    {
        isCrouch = false;
        isSprint = isSprintBeforeCrouch;

        //減速している速度をリセット
        MaxCrouchSpeed = DefaultMaxCrouchSpeed;
    }
    private void PerformSprintAction(InputAction.CallbackContext ctx)
    {
        isSprint = true;
        isCrouch = false;
    }
    private void CancelSpritAction(InputAction.CallbackContext ctx)
    {
        isSprint = false;
    }

    #endregion

    #region Methods_Collision

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            onGround = true;
            inputVector.y = 0;
        
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            onGround = false;
        }
    }

    #endregion

    #region Methos_static

    private void AddForceForward(Rigidbody rb, Transform transfrom, float3 inputVector)
    {
        //正面と右方向のベクトルを取得
        float3 forward = new(transfrom.forward.x, 0, transfrom.forward.z);
        float3 right = new(transfrom.right.x, 0, transfrom.right.z);

        //入力から計算する。
        var horizontalForce = right * inputVector.x;
        var verticalForce = forward * inputVector.z;

        //力を与える
        rb.AddForce(verticalForce);
        rb.AddForce(horizontalForce);

        //重力を与える。
        rb.AddForce(new(0, inputVector.y, 0), ForceMode.Acceleration);

        //状態に応じて挙動を変更
        if(onGround)
        {
            if(isCrouch)
            {
                //しゃがみ続けると減速
                if(MaxCrouchSpeed > 2.0f)
                {
                    MaxCrouchSpeed -= 0.1f;
                }
                //しゃがみ最大速度
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxCrouchSpeed);
            }
            else if(isSprint)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSprintSpeed);
            }
            //ダッシュでも、しゃがんでもないとき（歩き）
            else
            {
                //歩き最大速度
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
            }

            //慣性をなくす:しゃがんでいないときは慣性なし
            if (!isCrouch)
            {
                if (Vector3.Magnitude(horizontalForce) == 0)
                {
                    var localVelocity = transform.InverseTransformDirection(rb.velocity);
                    localVelocity.x = 0;
                    rb.velocity = transform.TransformDirection(localVelocity);
                }
                if (Vector3.Magnitude(verticalForce) == 0)
                {
                    var localVelocity = transform.InverseTransformDirection(rb.velocity);
                    localVelocity.z = 0;
                    rb.velocity = transform.TransformDirection(localVelocity);
                }
            }
        }
        text.text = "Speed:" + rb.velocity.magnitude.ToString();
    }

    #endregion
}
