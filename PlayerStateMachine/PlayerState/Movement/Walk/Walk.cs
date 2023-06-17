using Unity.Mathematics;
using UnityEngine;

public abstract class Walk : MovementState
{
    //設定項目
    protected float Speed { get; set; }
    protected float MaxSpeed { get; set; }
    protected float StraightBonus { get; set; }

    //フィールド
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
        //正面と右方向のベクトルを取得
        float3 forward = new(transform.forward.x, 0, transform.forward.z);
        float3 right = new(transform.right.x, 0, transform.right.z);

        //入力から計算する。
        horizontalForce = right * inputVector.x;
        verticalForce = forward * inputVector.z;

        //力を与える
        stateManager.rb.AddForce(verticalForce);
        stateManager.rb.AddForce(horizontalForce);

        var forceLocalDirection = transform.InverseTransformDirection(verticalForce);
        //方向によって速度ボーナスを与えれる。
        var isStraight = false;
        if (forceLocalDirection.y > 0)
        {
            isStraight = true;
        }
        var speed = MaxSpeed;
        //まっすぐ進んでいたら速度にボーナスを与える。
        if (isStraight)
        {
            speed *= StraightBonus;
        }

        //重力を与える。
        stateManager.rb.AddForce(new(0, inputVector.y, 0), ForceMode.Acceleration);
        //最大速度を設定
        stateManager.rb.velocity = Vector3.ClampMagnitude(stateManager.rb.velocity, speed);
    }

    public override void OnUpdate()
    {
        //移動
        float2 movementVector = stateManager.moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        Debug.Log(movementVector);
        var input = inputVector;
        input.x = movementVector.x;
        input.z = movementVector.y;
        inputVector = input;
    }
}
