using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
        //d—Í‚ð—^‚¦‚éB
        stateManager.rb.AddForce(new(0, Physics.gravity.y, 0), ForceMode.Acceleration);
        base.OnFixedUpdate();

        //•Ç‚Ì‚Ú‚è
        var offsetY = 0.5f;
        var distance = 3.0f;
        var direction = transform.forward;
        Debug.DrawRay(transform.position + Vector3.up * offsetY, direction, Color.red, distance + 0.5f);
        if(stateManager.rb.velocity.y > 0 && 1.5f > stateManager.rb.velocity.y)
        {
            if (Physics.Raycast(transform.position + Vector3.up * offsetY, direction, out var hit, distance + 0.5f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                stateManager.ChangeState(new ClimbWall(stateManager));
            }
        }
    }
    public override void OnEnterCollider(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            stateManager.ChangeState(new WalkOnGround(stateManager));
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}
