using UnityEngine;

public class WalkOnGround : Walk
{
    public WalkOnGround(CharacterStateManager stateManager) : base(stateManager)
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
        base.OnFixedUpdate();

        //äµê´ÇÇ»Ç≠Ç∑
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

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
