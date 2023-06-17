using Unity.Mathematics;
using UnityEngine;

public abstract class Walk : State
{
    //設定項目
    protected abstract float Speed { get; set; }
    protected abstract float StraightBonus { get; set; }

    //フィールド
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
        //正面と右方向のベクトルを取得
        float3 forward = new(transform.forward.x, 0, transform.forward.z);
        float3 right = new(transform.right.x, 0, transform.right.z);

        //入力から計算する。
        horizontalForce = right * inputVector.x;
        verticalForce = forward * inputVector.z;

        //力を与える
        rb.AddForce(verticalForce);
        rb.AddForce(horizontalForce);

        var forceLocalDirection = transform.InverseTransformDirection(verticalForce);
        //方向によって速度ボーナスを与えれる。
        var isStraight = false;
        if (forceLocalDirection.y > 0)
        {
            isStraight = true;
        }
        var speed = Speed;
        //まっすぐ進んでいたら速度にボーナスを与える。
        if (isStraight)
        {
            speed *= StraightBonus;
        }

        //重力を与える。
        rb.AddForce(new(0, inputVector.y, 0), ForceMode.Acceleration);
        //最大速度を設定
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
    }

    public override void OnUpdate()
    {
        //移動
        float2 movementVector = stateManager.moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        inputVector.x = movementVector.x;
        inputVector.z = movementVector.y;
    }
}
