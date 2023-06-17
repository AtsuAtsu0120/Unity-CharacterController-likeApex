using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected CharacterStateManager stateManager;
    public State(CharacterStateManager stateManager)
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
    public abstract void OnEnterCollider(Collider collider);
}
