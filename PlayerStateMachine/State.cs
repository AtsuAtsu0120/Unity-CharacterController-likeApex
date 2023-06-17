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
    public abstract void OnEnterCollider(Collider collider);
}
