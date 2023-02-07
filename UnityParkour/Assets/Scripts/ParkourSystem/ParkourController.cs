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

		var isObstacleFoward = _environmentScanner.ObstacleCheck(out var checkResult);
		if (!isObstacleFoward) 
		{
			return;
		}
		foreach (var action in _parkourActions) 
		{
			if (action.CheckIsPossible(checkResult, transform)) 
			{
				StartCoroutine(DoAction(action));
				break;
			}
		}
	}

	private IEnumerator DoAction(ParkourAction action)
	{
		_isInAction = true;
		_playerController.HasControl = false;
		_animator.CrossFade(action.StateName, 0.2f);

		yield return null;
		var animatorInfo = _animator.GetNextAnimatorStateInfo(0);
		if (!animatorInfo.IsName(action.StateName)) 
		{
			Debug.LogError($"[ParkourController.DoAction] actionName \"{action.StateName}\" not exist.");
		}
		var timer = 0f;

		while (timer <= animatorInfo.length)
		{
			timer += Time.deltaTime;
			if (action.IsRotateToObstacle) 
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, _playerController.RotateSpeed);
			}
			if (action.EnableTargetMatching) 
			{
				MatchTarget(action);
			}
			yield return null;
		}

		_playerController.HasControl = true;
		_isInAction = false;
	}

	private void MatchTarget(ParkourAction action) 
	{
		if (_animator.isMatchingTarget) 
		{
			return;
		}

		_animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBoyPart, 
			new MatchTargetWeightMask(Vector3.up, 0), action.MatchStartTime, action.MatchTargetTime);
	}
	#endregion private-method
}
