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
	private string _parameterName = "MoveAmount";
	[SerializeField]
	private CharacterController _characterController;

	[Header("GroundCheck")]
	[SerializeField]
	private float _groundCheckRadius;
	[SerializeField]
	private Vector3 _groundCheckOffset;
	[SerializeField]
	private LayerMask _groundCheckLayer;

	[SerializeField]
	private Vector3 _gravity = new Vector3(0, -9.8f, 0);

	private Transform _currentTransform;
	private Vector3 _fallSpeed = Vector3.zero;
	private bool _hasControl = true;
	#endregion private-field

	#region public-property
	public bool HasControl
	{
		set 
		{
			_hasControl = value;
			_characterController.enabled = _hasControl;

			if (_hasControl)
			{
				_playerAnimator?.SetFloat(_parameterName, 0);
			}
		}
	}

	public float RotateSpeed 
	{
		get 
		{
			return _rotateSpeed;
		}
	}
	#endregion public-property

	#region MonoBehaviour-method
	private void Awake()
	{
		CameraTransformProvider.Instance.OnGetTransform += OnGetTransform;
	}

	private void Update()
	{
		UpdatePos();
	}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
		var oldColor = Gizmos.color;
		Gizmos.color = Color.red;
		if (GroundCheck())
		{
			Gizmos.color = Color.green;
		}
		Gizmos.DrawSphere(transform.TransformPoint(_groundCheckOffset), _groundCheckRadius);

		Gizmos.color = oldColor;

	}
#endif

    private void OnDestroy()
	{
		CameraTransformProvider.Instance.OnGetTransform -= OnGetTransform;
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void UpdatePos() 
	{
		if (!_hasControl) 
		{
			return;
		}
		var h = Input.GetAxis("Horizontal");
		var v = Input.GetAxis("Vertical");

		var moveInput = (new Vector3(h, 0, v)).normalized;

		var isGround = GroundCheck();
		var moveDir = GetCameraRotation() * moveInput;

		var velocity = moveDir * _speed;
		if (!isGround)
		{
			_fallSpeed += _gravity * Time.deltaTime;
		}
		else 
		{
			_fallSpeed.y = -0.5f;
		}
		velocity.y = _fallSpeed.y;
		_characterController.Move(velocity * Time.deltaTime);

		LerpRotation(moveDir);

		_playerAnimator?.SetFloat(_parameterName, moveInput.magnitude, 0.2f, Time.deltaTime);
	}

	private bool GroundCheck() 
	{
		return Physics.CheckSphere(transform.TransformPoint(_groundCheckOffset), _groundCheckRadius, _groundCheckLayer);
	}

	private void LerpRotation(Vector3 moveDir)
	{
		if (moveDir.magnitude <= 0)
		{
			return;
		}
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
