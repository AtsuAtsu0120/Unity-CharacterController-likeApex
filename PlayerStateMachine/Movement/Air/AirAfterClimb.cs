using UnityEngine;

public class AirAfterClimb : Walk
{
    public AirAfterClimb(CharacterStateManager stateManager) : base(stateManager) { }

    public override void OnFixedUpdate()
    {
        //d—Í‚ğ—^‚¦‚éB
        stateManager.rb.AddForce(new(0, Physics.gravity.y, 0), ForceMode.Acceleration);
        base.OnFixedUpdate();
    }
    public override void OnEnterCollider(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            stateManager.ChangeState(new WalkOnGround(stateManager));
        }
    }
}
