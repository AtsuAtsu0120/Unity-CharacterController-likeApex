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
        //視点移動
        var lookVector = stateManager.lookAction.ReadValue<Vector2>();
        var camTransform = stateManager.transform.GetChild(0);
        //縦回転はカメラを動かす
        var camLocalAngle = stateManager.camTransform.localEulerAngles;
        camLocalAngle.x += -lookVector.y * stateManager.Sensibility;
        //縦回転は制限あり
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
        //横回転はPlayer自体を動かす
        localAngle.y += lookVector.x * stateManager.Sensibility;
        stateManager.transform.localEulerAngles = localAngle;
    }
}
