using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	#region private-field
	[SerializeField]
	private Transform _target;
	[SerializeField]
	private Vector3 _distanceFixer = new Vector3(0, 0, -5);

	[SerializeField]
	private float _rotateSpeed = 2;
	[SerializeField]
	private float _minVerticalAngle = -45;
	[SerializeField]
	private float _maxVerticalAngle = 45;

	[SerializeField]
	private bool _isInvertX = false;
	[SerializeField]
	private bool _isInvertY = false;

	private float _rotationX = 0;
	private float _rotationY = 0;
	#endregion private-field

	#region MonoBehaviour-method
	private void Awake()
	{
		CameraTransformProvider.Instance.SetTransform(transform);
	}

	private void Update()
	{
		SetCameraPos();
	}
	#endregion MonoBehaviour-method

	#region private-method
	private void SetCameraPos()
	{
		var invertXVal = _isInvertX ? -1 : 1;
		var invertYVal = _isInvertY ? -1 : 1;
		_rotationY += (Input.GetAxis("Camera X") * _rotateSpeed) * invertYVal;
		_rotationX -= (Input.GetAxis("Camera Y") * _rotateSpeed) * invertXVal;
		_rotationX = Mathf.Clamp(_rotationX, _minVerticalAngle, _maxVerticalAngle);
		var q = Quaternion.Euler(_rotationX, _rotationY, 0);

		transform.position = _target.position + q * _distanceFixer;
		transform.rotation = q;
	}
	#endregion private-method
}
