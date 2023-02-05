using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformProvider
{
	#region private-field
	private static CameraTransformProvider _instance;

	private Action<Transform> _onGetTransform;
	private Transform _cacheTransform;
	#endregion private-field

	#region public-property
	public static CameraTransformProvider Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new CameraTransformProvider();
			}
			return _instance;
		}
	}

	public event Action<Transform> OnGetTransform 
	{
		add 
		{
			_onGetTransform += value;
			_onGetTransform?.Invoke(_cacheTransform);
		}
		remove
		{
			_onGetTransform -= value;
		}
	}
	#endregion public-property

	#region public-method
	public void SetTransform(Transform transform) 
	{
		_cacheTransform = transform;
		_onGetTransform?.Invoke(_cacheTransform);
	}
	#endregion public-method
}
