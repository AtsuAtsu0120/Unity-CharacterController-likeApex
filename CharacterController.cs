using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
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

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI text;
    [Header("Layer")]
    [SerializeField] private LayerMask WallLayer;

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

    private bool isStraight = false; 

    #endregion
    #region Fields_Constant

    private readonly float MaxLimit = 85;
    private readonly float MinLimit = 360 - 85;
    private readonly float StraightBonus = 2;
    private float DefaultMaxCrouchSpeed;

    #endregion

    #region Methods_Unity
    private void Start()
    {
        //�J�[�\���̖�����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //InputSystem�̏�����
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
        //�����܂�

        //Rigidbody�̎擾
        rb = GetComponent<Rigidbody>();

        //�q�̃J�������擾
        camTransform = transform.GetChild(0).transform;
        standupCameraHeghit = camTransform.localPosition.y;

        //���Ⴊ��ł���Ƃ��̑��x��ۑ����Ă���
        DefaultMaxCrouchSpeed = MaxCrouchSpeed;
    }
    private void OnDisable()
    {
        actions.Disable();
        actions = null;
    }
    private void Update()
    {
        //���_�ړ�
        var lookVector = lookAction.ReadValue<Vector2>();
        var camTransfrom = transform.GetChild(0);
        //�c��]�̓J�����𓮂���
        var camLocalAngle = camTransform.localEulerAngles;
        camLocalAngle.x += -lookVector.y * Sensibility;
        //�c��]�͐�������
        if (camLocalAngle.x > MaxLimit && camLocalAngle.x < 180)
        {
            camLocalAngle.x = MaxLimit;
        }
        if (camLocalAngle.x < MinLimit && camLocalAngle.x > 180)
        {
            camLocalAngle.x = MinLimit;
        }
        camTransform.localEulerAngles = camLocalAngle;

        //����]��Player���̂𓮂���
        var localAngle = transform.localEulerAngles;
        localAngle.y += lookVector.x * Sensibility;
        transform.localEulerAngles = localAngle;

        //�ړ�
        float2 movementVector = moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        inputVector.x = movementVector.x;
        inputVector.z = movementVector.y;
    }
    private void FixedUpdate()
    {
        #region CharactorMoveUp

        GetWallStatus(0.5f, 1.0f, transform.forward);

        #endregion
        #region CharactorMovement

        //�d��
        if (!onGround)
        {
            inputVector.y = Physics.gravity.y;
        }

        //�X���C�f�B���O�̃N�[���^�C���̌���
        if (crouchCooltime >= 0.0f)
        {
            crouchCooltime -= Time.deltaTime;
        }

        AddForceForward(rb, transform, inputVector);

        #endregion

        #region CrounchCamera

        //���Ⴊ�݂̃J�����ړ��B
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

        //�������Ă��鑬�x�����Z�b�g
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

    #region Methods_Other

    private void AddForceForward(Rigidbody rb, Transform transfrom, float3 inputVector)
    {
        //���ʂƉE�����̃x�N�g�����擾
        float3 forward = new(transfrom.forward.x, 0, transfrom.forward.z);
        float3 right = new(transfrom.right.x, 0, transfrom.right.z);

        //���͂���v�Z����B
        var horizontalForce = right * inputVector.x;
        var verticalForce = forward * inputVector.z;

        //�͂�^����
        rb.AddForce(verticalForce);
        rb.AddForce(horizontalForce);

        //�����ɂ���đ��x�{�[�i�X��^�����B
        var forceLocalDirection = transform.InverseTransformDirection(verticalForce);
        if(forceLocalDirection.y < 0)
        {
            isStraight = true;
        }
        else
        {
            isStraight = false;             
        }

        //�d�͂�^����B
        rb.AddForce(new(0, inputVector.y, 0), ForceMode.Acceleration);

        //��Ԃɉ����ċ�����ύX
        if(onGround)
        {
            var speed = MaxSpeed;
            if(isCrouch)
            {
                //���Ⴊ�ݑ�����ƌ���
                if(MaxCrouchSpeed > 2.0f)
                {
                    MaxCrouchSpeed -= 0.1f;
                }
                //���Ⴊ�ݍő呬�x
                speed = MaxCrouchSpeed;
            }
            else if(isSprint)
            {
                //�_�b�V���̍ő呬�x
                speed = MaxSprintSpeed;
            }

            //�܂������i��ł����瑬�x�Ƀ{�[�i�X��^����B
            if(isStraight)
            {
                speed *= StraightBonus;
            }
            //�ő呬�x��ݒ�
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);

            //�������Ȃ���:���Ⴊ��ł��Ȃ��Ƃ��͊����Ȃ�
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
    private bool GetWallStatus(float offsetY, float distance, Vector3 direction)
    {
        Debug.DrawRay(transform.position + Vector3.up * offsetY, direction, Color.red, distance + 0.5f);
        if (Physics.Raycast(transform.position + Vector3.up * offsetY, direction, distance + 0.5f, WallLayer, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("�������Ă��܂��B");
            return true;
        }
        else
        {
            Debug.Log("�������Ă��܂���B");
            return false;
        }
    }

    #endregion
}
