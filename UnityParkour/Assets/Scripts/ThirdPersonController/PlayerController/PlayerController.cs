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

	[SerializeField]
	private EnvironmentScanner _environmentScanner;

	private Transform _currentTransform;
	private Vector3 _fallSpeed = Vector3.zero;
	private bool _hasControl = true;

	private Vector3 _desireMoveDir;
	private Vector3 _moveDir;
	private Vector3 _velocity;
	private Quaternion _targetRotation;
	#endregion private-field

	#region public-property
	public bool HasControl
	{
		set
		{
			_hasControl = value;
			_characterController.enabled = _hasControl;

			if (!_hasControl)
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

	public bool IsOnLedge
	{
		get;
		set;
	}

	public LedgeHitData LedgeHitData
	{
		get;
		private set;
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
		var moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

		var isGround = GroundCheck();
		_playerAnimator.SetBool("IsGround", isGround);
		_desireMoveDir = GetCameraRotation() * moveInput;
		_moveDir = _desireMoveDir;

		_velocity = Vector3.zero;
		if (!isGround)
		{
			_fallSpeed += _gravity * Time.deltaTime;

			_velocity = transform.forward * _speed / 2;
		}
		else
		{
			_velocity = _desireMoveDir * _speed;
			_fallSpeed.y = -0.5f;

			LedgeCheck(_desireMoveDir);

			_playerAnimator?.SetFloat(_parameterName, _velocity.magnitude / _speed, 0.2f, Time.deltaTime);
		}
		_velocity.y = _fallSpeed.y;
		_characterController.Move(_velocity * Time.deltaTime);

		LerpRotation(moveAmount, _moveDir);

	}

	private bool GroundCheck() 
	{
		var res = Physics.CheckSphere(transform.TransformPoint(_groundCheckOffset), _groundCheckRadius, _groundCheckLayer);
		return res;
	}

	private void LedgeCheck(Vector3 moveDir) 
	{
		if (_environmentScanner == null) 
		{
			return;
		}
		IsOnLedge = _environmentScanner.LedgeCheck(moveDir, out var ledgeHitData);
		if (IsOnLedge) 
		{
			LedgeHitData = ledgeHitData;
			LedgeMovement();
		}
	}

	private void LedgeMovement()
	{
		var signedAngle = Vector3.SignedAngle(LedgeHitData.SurfaceHitInfo.normal, _desireMoveDir, Vector3.up);
		var angle = Mathf.Abs(signedAngle);
		if (Vector3.Angle(_desireMoveDir, transform.forward) >= 80) 
		{
			_velocity = Vector3.zero;
			return;
		}
		if (angle < 60)
		{
			_velocity = Vector3.zero;
			_moveDir = Vector3.zero;
		}
		else if (angle < 90)
		{
			var left = Vector3.Cross(Vector3.up, LedgeHitData.SurfaceHitInfo.normal);
			var dir = left * Mathf.Sign(signedAngle);
			_velocity = _velocity.magnitude * dir;
			_moveDir = dir;
		}
	}

	private void LerpRotation(float moveAmount, Vector3 moveDir)
	{
		if (moveAmount > 0 && moveDir.magnitude > 0.2f)
		{
			_targetRotation = Quaternion.LookRotation(moveDir);
		}
		var cur = transform.rotation;
		var res = Quaternion.RotateTowards(cur, _targetRotation, _rotateSpeed * Time.deltaTime);
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
