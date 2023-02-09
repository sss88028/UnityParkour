using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	[Header("Ledge")]
	[SerializeField]
	private float _ledgeRayLength;
	[SerializeField]
	private float _ledgeHeightThreshold = 0.75f;
	#endregion private-field

	#region public-method
	public bool ObstacleCheck(out ObstacleHitData hitData) 
	{
		hitData = new ObstacleHitData();
		var origin = transform.position + _forwardRayOffset;
		var rayDir = transform.forward;
		hitData.MoveDir = rayDir;

		var isHit = Physics.Raycast(origin, rayDir, out hitData.ForwardHitInfo, _forwardRayLength, _obstacleLayer);
		Debug.DrawRay(origin, rayDir * _forwardRayLength, isHit ? Color.red : Color.white);

		if (isHit)
		{
			var hightOrigin = hitData.ForwardHitInfo.point + Vector3.up * _heightRayLength;
			var hightrayDir = Vector3.down;
			Physics.Raycast(hightOrigin, Vector3.down, out hitData.HeightHitInfo, _heightRayLength, _obstacleLayer);
			Debug.DrawRay(hightOrigin, hightrayDir * _heightRayLength, isHit ? Color.red : Color.white);
		}

		return isHit;
	}

	public bool LedgeCheck(Vector3 moveDir, out LedgeHitData ledgeData) 
	{
		ledgeData = new LedgeHitData();
		if (moveDir == Vector3.zero)
		{
			return false;
		}
		var originOffset = 0.5f;
		var origin = transform.position + moveDir * originOffset + Vector3.up;

		var isHit = PhysicsUtil.ThreeRaycasts(origin, Vector3.down, 0.25f, transform, 
			out var hitDatas, _ledgeRayLength, _obstacleLayer, true);
		if (isHit)
		{
			var resHitData = from h in hitDatas
							 where transform.position.y - h.point.y > _ledgeHeightThreshold
							 select h;
			
			if (resHitData.Any())
			{
				var surfaceRayOrigin = resHitData.FirstOrDefault().point;
				surfaceRayOrigin.y = transform.position.y - 0.1f;
				var dir = transform.position - surfaceRayOrigin;
				var isHitSurface = Physics.Raycast(surfaceRayOrigin, dir, out var surfaceHit, 2, _obstacleLayer);

				Debug.DrawLine(surfaceRayOrigin, transform.position, isHitSurface ? Color.cyan : Color.white);
				if (isHitSurface)
				{
					var height = transform.position.y - resHitData.FirstOrDefault().point.y;

					ledgeData.Angle = Vector3.Angle(transform.forward, surfaceHit.normal);
					ledgeData.Height = height;
					ledgeData.SurfaceHitInfo = surfaceHit;
					return true;
				}
			}
		}

		return false;
	}
	#endregion public-method
}

public struct ObstacleHitData 
{
	public bool IsHit;
	public Vector3 MoveDir;
	public RaycastHit ForwardHitInfo;
	public RaycastHit HeightHitInfo;
}

public struct LedgeHitData
{
	public float Height;
	public float Angle;
	public RaycastHit SurfaceHitInfo;
}