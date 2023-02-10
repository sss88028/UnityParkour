using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStoppingAction : StateMachineBehaviour
{
    #region private-field
    private PlayerController _playerController;
    #endregion private-field

    #region public-method
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (_playerController == null)
        {
            _playerController = animator.GetComponent<PlayerController>();
        }
        if (_playerController != null)
        {
            _playerController.HasControl = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (_playerController == null)
        {
            _playerController = animator.GetComponent<PlayerController>();
        }
        if (_playerController != null)
        {
            _playerController.HasControl = true;
        }
    }
    #endregion public-method
}
