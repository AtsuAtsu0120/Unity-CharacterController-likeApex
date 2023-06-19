using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MovementState
{
    protected CharacterStateManager stateManager { get; private set; }
    public MovementState(CharacterStateManager stateManager)
    {
        this.stateManager = stateManager;
    }
    /// <summary>
    /// このステートに切り替わったとき
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// このステートの時のアップデート
    /// </summary>
    public abstract void OnUpdate();
    /// <summary>
    /// このステートのときのフィックスドアップデート
    /// </summary>
    public abstract void OnFixedUpdate();
    /// <summary>
    /// このステートに切り替わったとき
    /// </summary>
    public abstract void OnExit();
    /// <summary>
    /// 衝突判定
    /// </summary>
    /// <param name="collider"></param>
    public abstract void OnEnterCollider(Collision collision);
    public abstract void OnPerformUp(InputAction.CallbackContext ctx);
    public abstract void OnPerformDown(InputAction.CallbackContext ctx);
    public abstract void OnCancelDown(InputAction.CallbackContext ctx);
    public abstract void OnPerformSprint(InputAction.CallbackContext ctx);
    public abstract void OnCancelSprint(InputAction.CallbackContext ctx);
}
