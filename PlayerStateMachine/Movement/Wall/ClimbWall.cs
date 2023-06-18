using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbWall : MovementState
{
    public ClimbWall(CharacterStateManager stateManager) : base(stateManager) { }

    private float MaxClimbSpeed = 5.0f;
    private float Speed = 50.0f;
    protected Vector3 inputVector { get; set; }
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        if (MaxClimbSpeed < 0)
        {
            stateManager.ChangeState(new AirAfterClimb(stateManager));
        }
        else
        {
            MaxClimbSpeed -= 0.05f;
            //��ƉE�����̃x�N�g�����擾
            float3 up = new(0, stateManager.transform.up.y, 0);
            float3 right = new(stateManager.transform.right.x, 0, stateManager.transform.right.z);

            //���͂���v�Z����B
            var horizontalForce = right * inputVector.x;
            var upForce = up * inputVector.z;
            stateManager.rb.AddForce(upForce);
            stateManager.rb.AddForce(horizontalForce);
            //�ő呬�x��ݒ�
            stateManager.rb.velocity = Vector3.ClampMagnitude(stateManager.rb.velocity, MaxClimbSpeed); 
        }
    }
    public override void OnUpdate()
    {
        //�ړ�
        float2 movementVector = stateManager.moveAction.ReadValue<Vector2>();

        movementVector *= Speed;
        var input = inputVector;
        input.x = movementVector.x;
        input.z = movementVector.y;
        inputVector = input;
    }
    public override void OnEnterCollider(Collision collision)
    {
        
    }

    public override void OnPerformDown(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnPerformSprint(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnPerformUp(InputAction.CallbackContext ctx)
    {
        
    }

    public override void OnCancelDown(InputAction.CallbackContext ctx)
    {
        stateManager.ChangeState(new AirAfterClimb(stateManager));
    }

    public override void OnCancelSprint(InputAction.CallbackContext ctx)
    {
        
    }
}
