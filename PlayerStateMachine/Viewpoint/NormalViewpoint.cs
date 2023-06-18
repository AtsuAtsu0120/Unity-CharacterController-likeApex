using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalViewpoint : ViewpointState
{
    public NormalViewpoint(CharacterStateManager stateManager) : base(stateManager)
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
        
    }

    public override void OnUpdate()
    {
        //Ž‹“_ˆÚ“®
        var lookVector = stateManager.lookAction.ReadValue<Vector2>();
        var camTransform = stateManager.transform.GetChild(0);
        //c‰ñ“]‚ÍƒJƒƒ‰‚ð“®‚©‚·
        var camLocalAngle = stateManager.camTransform.localEulerAngles;
        camLocalAngle.x += -lookVector.y * stateManager.Sensibility;
        //c‰ñ“]‚Í§ŒÀ‚ ‚è
        if (camLocalAngle.x > stateManager.MaxLimit && camLocalAngle.x < 180)
        {
            camLocalAngle.x = stateManager.MaxLimit;
        }
        if (camLocalAngle.x < stateManager.MinLimit && camLocalAngle.x > 180)
        {
            camLocalAngle.x = stateManager.MinLimit;
        }
        camTransform.localEulerAngles = camLocalAngle;
        var localAngle = stateManager.transform.localEulerAngles;
        //‰¡‰ñ“]‚ÍPlayerŽ©‘Ì‚ð“®‚©‚·
        localAngle.y += lookVector.x * stateManager.Sensibility;
        stateManager.transform.localEulerAngles = localAngle;
    }
}
