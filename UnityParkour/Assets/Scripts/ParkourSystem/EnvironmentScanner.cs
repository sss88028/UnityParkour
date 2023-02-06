using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentScanner : MonoBehaviour
{
	#region private-field
	[Header("Forward")]
	[SerializeField]
	private Vector3 _forwardRayOffset = new Vector3(0, 2.5f, 0);
	[SerializeField]
	private float _forwardRayLength = 0.8f;
	[SerializeField]
	private LayerMask _obstacleLayer;
	[Header("Height")]
	[SerializeField]
	private float _heightRayLength = 5f;
	#endregion private-field

	#region public-method
	public bool ObstacleCheck(out RaycastHit fowardHitInfo, out RaycastHit heightHitInfo) 
	{
		var origin = transform.position + _forwardRayOffset;
		var rayDir = transform.forward;
		
		var isHit = Physics.Raycast(origin, rayDir, out fowardHitInfo, _forwardRayLength, _obstacleLayer);
		Debug.DrawRay(origin, rayDir * _forwardRayLength, isHit ? Color.red : Color.white);

		if (isHit)
		{
			var hightOrigin = fowardHitInfo.point + Vector3.up * _heightRayLength;
			var hightrayDir = Vector3.down;
			Physics.Raycast(hightOrigin, Vector3.down, out heightHitInfo, _heightRayLength, _obstacleLayer);
			Debug.DrawRay(hightOrigin, hightrayDir * _heightRayLength, isHit ? Color.red : Color.white);
		}
		else 
		{
			heightHitInfo = default;
		}

		return isHit;
	}
	#endregion public-method
}