using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
	#region private-field
	[SerializeField]
	private Vector3 _forwardRayOffset = new Vector3(0, 2.5f, 0);
	[SerializeField]
	private float _forwardRayLength = 0.8f;
	[SerializeField]
	private LayerMask _obstacleLayer;
	#endregion private-field

	#region public-method
	public bool ObstacleCheck(out RaycastHit hitInfo) 
	{
		var origin = transform.position + _forwardRayOffset;
		var rayDir = transform.forward;
		var isHit = Physics.Raycast(origin, rayDir, out hitInfo, _forwardRayLength, _obstacleLayer);

		Debug.DrawRay(origin, rayDir * _forwardRayLength, isHit ? Color.red : Color.white);
		return isHit;
	}
	#endregion public-method
}