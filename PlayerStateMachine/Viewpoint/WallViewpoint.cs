using UnityEngine;

public class WallViewpoint : NormalViewpoint
{
    private RaycastHit hit;
    private float limitY;
    public WallViewpoint(CharacterStateManager stateManager, RaycastHit hit) : base(stateManager) { this.hit = hit; }

    public override void OnEnter()
    {
        //•Ç‚Ì•ûŒü‚ðŒ©‚éB
        var direction = hit.transform.position - stateManager.transform.position;
        direction.y = 0;
        var lookAngle = Quaternion.LookRotation(direction);
        stateManager.transform.rotation = Quaternion.Lerp(stateManager.transform.rotation, lookAngle, 0.1f);
        limitY = stateManager.transform.localEulerAngles.y;
    }

    public override void OnExit()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var localAngle = stateManager.transform.localEulerAngles;

        var angleY = localAngle.y - limitY;

        if (angleY > 30 && angleY < 90)
        {
            localAngle.y = 30 + limitY;
        }
        if (angleY < -30 && angleY > -90)
        {
            localAngle.y = limitY - 30;
        }
        stateManager.transform.localEulerAngles = localAngle;
    }
}
