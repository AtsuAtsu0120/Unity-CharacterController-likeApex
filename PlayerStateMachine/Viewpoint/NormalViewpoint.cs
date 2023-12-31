using UnityEngine;

public class NormalViewpoint : ViewpointState
{
    private float Sensibility = 1.0f;
    private float MaxLimit = 85.0f;
    private float MinLimit = 0.0f;
    public NormalViewpoint(CharacterStateManager stateManager) : base(stateManager)
    {

    }

    public override void OnEnter()
    {
        MinLimit = 360 - MaxLimit;
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
        camLocalAngle.x += -lookVector.y * Sensibility;
        //縦回転は制限あり
        if (camLocalAngle.x > MaxLimit && camLocalAngle.x < 180)
        {
            camLocalAngle.x = MaxLimit;
        }
        if (camLocalAngle.x < MinLimit && camLocalAngle.x > 180)
        {
            camLocalAngle.x = MinLimit;
        }
        camTransform.localEulerAngles = camLocalAngle;
        var localAngle = stateManager.transform.localEulerAngles;
        //横回転はPlayer自体を動かす
        localAngle.y += lookVector.x * Sensibility;
        stateManager.transform.localEulerAngles = localAngle;
    }
}
