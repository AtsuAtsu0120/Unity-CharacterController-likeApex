using UnityEngine.InputSystem;

public class Crouch : Walk
{
    public Crouch(CharacterStateManager stateManager) : base(stateManager) { }

    private float MinSpeed = 2.0f;
    public override void OnEnter()
    {
        Speed = 20.0f;
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if(Speed > MinSpeed)
        {
            Speed -= 0.1f;
        }
    }
    public override void OnCancelDown(InputAction.CallbackContext ctx)
    {
        stateManager.ChangeState(new WalkOnGround(stateManager));
    }

}
