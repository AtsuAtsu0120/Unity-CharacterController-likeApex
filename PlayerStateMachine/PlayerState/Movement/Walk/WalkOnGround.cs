using UnityEngine;
using UnityEngine.InputSystem;

public class WalkOnGround : Walk
{
    public WalkOnGround(CharacterStateManager stateManager) : base(stateManager)
    {

    }

    public override void OnEnter()
    {
        Speed = 50.0f;
        MaxSpeed = 15.0f;
    }

    public override void OnEnterCollider(Collider collider)
    {
        
    }

    public override void OnExit()
    {

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        //äµê´ÇÇ»Ç≠Ç∑
        if (Vector3.Magnitude(horizontalForce) == 0)
        {
            var localVelocity = transform.InverseTransformDirection(stateManager.rb.velocity);
            localVelocity.x = 0;
            stateManager.rb.velocity = transform.TransformDirection(localVelocity);
        }
        if (Vector3.Magnitude(verticalForce) == 0)
        {
            var localVelocity = transform.InverseTransformDirection(stateManager.rb.velocity);
            localVelocity.z = 0;
            stateManager.rb.velocity = transform.TransformDirection(localVelocity);
        }
    }

    public override void OnPerformUp(InputAction.CallbackContext ctx)
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
