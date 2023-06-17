using Unity.Mathematics;
using UnityEngine;

public abstract class Walk : State
{
    //�ݒ荀��
    protected abstract float Speed { get; set; }
    protected abstract float StraightBonus { get; set; }

    //�t�B�[���h
    protected Vector3 horizontalForce { get; }
    protected Vector3 verticalForce { get; }
    protected Vector3 inputVector { get; }
    protected Transform transform { get; }
    protected Rigidbody rb { get; }
    public Walk(CharacterStateManager stateManager) : base(stateManager)
    {
        
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        transform = stateManager.transform;
        rb = stateManager.rb;
        //���ʂƉE�����̃x�N�g�����擾
        float3 forward = new(transform.forward.x, 0, transform.forward.z);
        float3 right = new(transform.right.x, 0, transform.right.z);

        //���͂���v�Z����B
        horizontalForce = right * inputVector.x;
        verticalForce = forward * inputVector.z;

        //�͂�^����
        rb.AddForce(verticalForce);
        rb.AddForce(horizontalForce);

        var forceLocalDirection = transform.InverseTransformDirection(verticalForce);
        //�����ɂ���đ��x�{�[�i�X��^�����B
        var isStraight = false;
        if (forceLocalDirection.y > 0)
        {
            isStraight = true;
        }
        var speed = Speed;
        //�܂������i��ł����瑬�x�Ƀ{�[�i�X��^����B
        if (isStraight)
        {
            speed *= StraightBonus;
        }

        //�d�͂�^����B
        rb.AddForce(new(0, inputVector.y, 0), ForceMode.Acceleration);
        //�ő呬�x��ݒ�
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
    }

    public override void OnUpdate()
    {
        //�ړ�
        float2 movementVector = stateManager.moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        inputVector.x = movementVector.x;
        inputVector.z = movementVector.y;
    }
}
