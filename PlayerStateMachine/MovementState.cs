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
    /// ���̃X�e�[�g�ɐ؂�ւ�����Ƃ�
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// ���̃X�e�[�g�̎��̃A�b�v�f�[�g
    /// </summary>
    public abstract void OnUpdate();
    /// <summary>
    /// ���̃X�e�[�g�̂Ƃ��̃t�B�b�N�X�h�A�b�v�f�[�g
    /// </summary>
    public abstract void OnFixedUpdate();
    /// <summary>
    /// ���̃X�e�[�g�ɐ؂�ւ�����Ƃ�
    /// </summary>
    public abstract void OnExit();
    /// <summary>
    /// �Փ˔���
    /// </summary>
    /// <param name="collider"></param>
    public abstract void OnEnterCollider(Collision collision);
    public abstract void OnPerformUp(InputAction.CallbackContext ctx);
    public abstract void OnPerformDown(InputAction.CallbackContext ctx);
    public abstract void OnCancelDown(InputAction.CallbackContext ctx);
    public abstract void OnPerformSprint(InputAction.CallbackContext ctx);
    public abstract void OnCancelSprint(InputAction.CallbackContext ctx);
}
