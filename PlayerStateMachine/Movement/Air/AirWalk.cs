using UnityEngine;
using UnityEngine.InputSystem;

public class AirWalk : Walk
{
    public AirWalk(CharacterStateManager stateManager) : base(stateManager)
    {

    }
    public override void OnEnter()
    {
        Debug.Log("Air");
    }
    public override void OnFixedUpdate()
    {
        //重力を与える。
        stateManager.rb.AddForce(new(0, Physics.gravity.y, 0), ForceMode.Acceleration);
        base.OnFixedUpdate();

        //壁のぼり
        var offsetY = 0.5f;
        var distance = 3.0f;
        var direction = transform.forward;
        Debug.DrawRay(transform.position + Vector3.up * offsetY, direction, Color.red, distance + 0.5f);
        if(stateManager.rb.velocity.y > 0 && 1.5f > stateManager.rb.velocity.y)
        {
            if (Physics.Raycast(transform.position + Vector3.up * offsetY, direction, out var hit, distance + 0.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                stateManager.ChangeState(new ClimbWall(stateManager, hit));
                stateManager.ChangeViewPointState(new WallViewpoint(stateManager));
            }
        }
    }
    public override void OnPerformDown(InputAction.CallbackContext ctx)
    {
        stateManager.ChangeState(new AirCrouch(stateManager));
    }
    public override void OnEnterCollider(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            stateManager.ChangeState(new WalkOnGround(stateManager));
        }
    }
}
