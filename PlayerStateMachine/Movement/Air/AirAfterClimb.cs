using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AirAfterClimb : Walk
{
    public AirAfterClimb(CharacterStateManager stateManager) : base(stateManager) { }

    public override void OnFixedUpdate()
    {
        //èdóÕÇó^Ç¶ÇÈÅB
        stateManager.rb.AddForce(new(0, Physics.gravity.y, 0), ForceMode.Acceleration);
        base.OnFixedUpdate();
    }
    public override void OnEnterCollider(Collision collision)
    {
        stateManager.ChangeState(new WalkOnGround(stateManager));
    }
}
