using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtil
{
	#region public-method
	public static bool ThreeRaycasts(Vector3 origin, Vector3 dir, float spacing, Transform transform, 
		out IReadOnlyList<RaycastHit> hits, float distance, LayerMask layerMask, bool debugDraw = false) 
	{
		var isCenterHit = Physics.Raycast(origin, dir, out var centerHit, distance, layerMask);
		var isLeftHit = Physics.Raycast(origin - transform.right * spacing, dir, out var leftHit, distance, layerMask);
		var isRightHit = Physics.Raycast(origin + transform.right * spacing, dir, out var rightHit, distance, layerMask);

		hits = new List<RaycastHit>()
		{
			centerHit,
			leftHit,
			rightHit
		};
		var isHit = isCenterHit || isLeftHit || isRightHit;

		if (debugDraw) 
		{
			var hitEnd = isCenterHit ? centerHit.point : dir * 5;
			Debug.DrawLine(origin, hitEnd, isCenterHit ? Color.red : Color.white);

			hitEnd = isLeftHit ? leftHit.point : dir * 5;
			Debug.DrawLine(origin - transform.right * spacing, hitEnd, isLeftHit ? Color.red : Color.white);

			hitEnd = isRightHit ? rightHit.point : dir * 5;
			Debug.DrawLine(origin + transform.right * spacing, hitEnd, isRightHit ? Color.red : Color.white);
		}
		return isHit;
	}
	#endregion public-method
}
