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
	private Animator _animator;
	[SerializeField]
	private string _stateName = "StepUp";

	private bool _isInAction = false;
	#endregion private-field

	#region MonoBehaviour-method
	private void Update()
	{
		CheckEnvironment();
	}
	#endregion MonoBehaviour-method

	#region private-method
	private async void CheckEnvironment() 
	{
		if (_environmentScanner == null) 
		{
			return;
		}

		if (!Input.GetButton("Jump") || _isInAction) 
		{
			return;
		}

		var isObstacleFoward = _environmentScanner.ObstacleCheck(out var hitInfo, out var hieghtInfo);
		if (!isObstacleFoward) 
		{
			return;
		}
		_isInAction = true;
		_playerController.HasControl = false;
		_animator.CrossFade(_stateName, 0.2f);

		await Task.Yield();
		var animatorInfo = _animator.GetNextAnimatorStateInfo(0);

		await Task.Delay((int)(animatorInfo.length * 1000));
		_playerController.HasControl = true;
		_isInAction = false;
	}
	#endregion private-method
}
