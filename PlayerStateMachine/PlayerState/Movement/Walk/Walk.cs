using Unity.Mathematics;
using UnityEngine;

public abstract class Walk : MovementState
{
    //�ݒ荀��
    protected float Speed { get; set; }
    protected float MaxSpeed { get; set; }
    protected float StraightBonus { get; set; }

    //�t�B�[���h
    protected Vector3 horizontalForce { get; private set; }
    protected Vector3 verticalForce { get; private set; }
    protected Vector3 inputVector { get; private set; }
    protected Transform transform { get; private set; }
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
        //���ʂƉE�����̃x�N�g�����擾
        float3 forward = new(transform.forward.x, 0, transform.forward.z);
        float3 right = new(transform.right.x, 0, transform.right.z);

        //���͂���v�Z����B
        horizontalForce = right * inputVector.x;
        verticalForce = forward * inputVector.z;

        //�͂�^����
        stateManager.rb.AddForce(verticalForce);
        stateManager.rb.AddForce(horizontalForce);

        var forceLocalDirection = transform.InverseTransformDirection(verticalForce);
        //�����ɂ���đ��x�{�[�i�X��^�����B
        var isStraight = false;
        if (forceLocalDirection.y > 0)
        {
            isStraight = true;
        }
        var speed = MaxSpeed;
        //�܂������i��ł����瑬�x�Ƀ{�[�i�X��^����B
        if (isStraight)
        {
            speed *= StraightBonus;
        }

        //�d�͂�^����B
        stateManager.rb.AddForce(new(0, inputVector.y, 0), ForceMode.Acceleration);
        //�ő呬�x��ݒ�
        stateManager.rb.velocity = Vector3.ClampMagnitude(stateManager.rb.velocity, speed);
    }

    public override void OnUpdate()
    {
        //�ړ�
        float2 movementVector = stateManager.moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        Debug.Log(movementVector);
        var input = inputVector;
        input.x = movementVector.x;
        input.z = movementVector.y;
        inputVector = input;
    }
}
