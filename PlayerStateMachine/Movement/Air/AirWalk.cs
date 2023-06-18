using System.Collections;
using System.Collections.Generic;
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
        //èdóÕÇó^Ç¶ÇÈÅB
        stateManager.rb.AddForce(new(0, Physics.gravity.y, 0), ForceMode.Acceleration);
        base.OnFixedUpdate();
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
