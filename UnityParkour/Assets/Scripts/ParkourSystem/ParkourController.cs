using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
	#region private-field
	[SerializeField]
	private EnvironmentScanner _environmentScanner;
	[SerializeField]
	private PlayerController _playerController;
	[SerializeField]
	private List<ParkourAction> _parkourActions = new List<ParkourAction>();
	[SerializeField]
	private ParkourAction _jumpDownAction;
	[SerializeField]
	private float _autoJumpDonwHeight = 1;

	[SerializeField]
	private Animator _animator;
	#endregion private-field

	#region MonoBehaviour-method
	private void Update()
	{
		CheckParkourAction();
		CheckJumpDown();
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void CheckParkourAction() 
	{
		if (_environmentScanner == null) 
		{
			return;
		}

		if (!Input.GetButton("Jump") || _playerController.IsInAction) 
		{
			return;
		}
		if (_playerController.IsHanging)
		{
			return;
		}

		var isObstacleFoward = _environmentScanner.ObstacleCheck(out var checkResult);
		if (!isObstacleFoward) 
		{
			return;
		}
		foreach (var action in _parkourActions) 
		{
			if (action.CheckIsPossible(checkResult, transform)) 
			{
				DoAction(action);
				break;
			}
		}
	}

	private void CheckJumpDown()
	{
		if (_playerController.IsInAction)
		{
			return;
		}
		var isObstacleFoward = _environmentScanner.ObstacleCheck(out var _);
		if (isObstacleFoward)
		{
			return;
		}
		if (!_playerController.IsOnLedge)
		{
			return;
		}
		if (_playerController.LedgeHitData.Angle > 50) 
		{
			return;
		}
		var canJump = _playerController.LedgeHitData.Height < _autoJumpDonwHeight;

		if (!canJump)
		{
			if (!Input.GetButton("Jump"))
			{
				return;
			}
		}
		_playerController.IsOnLedge = false;
		DoAction(_jumpDownAction);
	}

	private async void DoAction(ParkourAction action)
	{
		_playerController.SetControl(false);
		var matchParams = default(MatchTargetParams);
		if (action.EnableTargetMatching)
		{
			matchParams = action;
		}
		await _playerController.DoAction(action.StateName, action.FinishStateName, matchParams, action.TargetRotation, action.IsRotateToObstacle);
		_playerController.SetControl(true);
	}

	private void MatchTarget(ParkourAction action) 
	{
		if (_animator.isMatchingTarget) 
		{
			return;
		}
		_animator.SetBool("VaultMirror", action.IsMirror);
		_animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBoyPart, 
			new MatchTargetWeightMask(action.MathcWeight, 0), action.MatchStartTime, action.MatchTargetTime);
	}
	#endregion private-method
}
