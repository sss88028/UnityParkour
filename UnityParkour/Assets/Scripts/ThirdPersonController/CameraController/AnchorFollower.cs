using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorFollower : MonoBehaviour
{
	#region private-field
	[SerializeField]
	private Transform _target;
	[SerializeField]
	private float _speed = 1000;
	[SerializeField]
	private float _rotateSpeed = 1000;
	#endregion private-field

	#region MonoBehaviour-method
	private void LateUpdate()
	{
		FollowPosition();
		FollowRotation();
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void FollowPosition()
	{
		if (_target == null)
		{
			return;
		}
		var targetPos = _target.position;
		var currentPos = transform.position;
		var resPos = currentPos;
		if (targetPos == currentPos)
		{
			resPos = targetPos;
		}
		else 
		{
			resPos = Vector3.Lerp(currentPos, targetPos, _speed * Time.deltaTime);
		}
		transform.position = resPos;
	}

	private void FollowRotation()
	{
		if (_target == null)
		{
			return;
		}
		var targetRot = _target.rotation;
		var currentRot = transform.rotation;

		var resRot = currentRot;
		if (targetRot == currentRot)
		{
			resRot = currentRot;
		}
		else
		{
			resRot = Quaternion.Lerp(currentRot, targetRot, _rotateSpeed * Time.deltaTime);
		}
		transform.rotation = resRot;
	}
	#endregion private-method
}
