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
	private Animator _animator;

	private bool _isInAction = false;
	#endregion private-field

	#region MonoBehaviour-method
	private void Update()
	{
		CheckEnvironment();
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void CheckEnvironment() 
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
		foreach (var action in _parkourActions) 
		{
			if (action.CheckIsPossible(hieghtInfo, transform)) 
			{
				DoAction(action.StateName);
				break;
			}
		}
	}

	private async void DoAction(string actionName)
	{
		_isInAction = true;
		_playerController.HasControl = false;
		_animator.CrossFade(actionName, 0.2f);

		await Task.Yield();
		var animatorInfo = _animator.GetNextAnimatorStateInfo(0);
		if (!animatorInfo.IsName(actionName)) 
		{
			Debug.LogError($"[ParkourController.DoAction] actionName \"{actionName}\" not exist.");
		}

		var waitTime = (int)(animatorInfo.length * 1000);
		await Task.Delay(waitTime);
		_playerController.HasControl = true;
		_isInAction = false;
	}
	#endregion private-method
}
