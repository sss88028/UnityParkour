using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region private-field
	[SerializeField]
	private float _speed = 5;
	[SerializeField]
	private float _rotateSpeed = 500f;
	[SerializeField]
	private Animator _playerAnimator;
	[SerializeField]
	private string _paramertName = "MoveAmount";

	private Transform _currentTransform;
	#endregion private-field

	#region MonoBehaviour-method
	private void Awake()
	{
		CameraTransformProvider.Instance.OnGetTransform += OnGetTransform;
	}

	private void Update()
	{
		UpdatePos();
	}

	private void OnDestroy()
	{
		CameraTransformProvider.Instance.OnGetTransform -= OnGetTransform;
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void UpdatePos() 
	{
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");

		var moveAmount = Math.Abs(h)+ Math.Abs(v);
		var moveInput = (new Vector3(h, 0, v)).normalized;

		if (moveAmount > 0)
		{
			var moveDir = GetCameraRotation() * moveInput;

			transform.position += moveDir * _speed * Time.deltaTime;
			LerpRotation(moveDir);
		}

		_playerAnimator?.SetFloat(_paramertName, moveInput.magnitude, 0.2f, Time.deltaTime);

	}

	private void LerpRotation(Vector3 moveDir)
	{
		var cur = transform.rotation;
		var target = Quaternion.LookRotation(moveDir);
		var res = Quaternion.RotateTowards(cur, target, _rotateSpeed * Time.deltaTime);
		transform.rotation = res;
	}

	private void OnGetTransform(Transform transform) 
	{
		_currentTransform = transform;
	}

	private Quaternion GetCameraRotation() 
	{
		if (_currentTransform == null) 
		{
			return Quaternion.identity;
		}
		var euler = _currentTransform.rotation.eulerAngles;
		return Quaternion.Euler(0, euler.y, 0);
	}
	#endregion private-method
}
