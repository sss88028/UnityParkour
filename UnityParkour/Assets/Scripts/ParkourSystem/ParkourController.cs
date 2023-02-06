using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
	#region private-field
	[SerializeField]
	private EnvironmentScanner _environmentScanner;
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
		_environmentScanner.ObstacleCheck(out var hitInfo);
	}
	#endregion private-method
}
