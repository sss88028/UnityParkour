using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New Vault Action", fileName = "VaultAction")]
public class VaultAction : ParkourAction
{
	#region public-method
	public override bool CheckIsPossible(ObstacleHitData hitData, Transform player)
	{
		var res = base.CheckIsPossible(hitData, player);
		if (!res) 
		{
			return res;
		}
		var moveDir = hitData.ForwardHitInfo.transform.InverseTransformVector(hitData.MoveDir);
		var leftDir = Vector3.Cross(moveDir, Vector3.up);
		var hitPoint = hitData.ForwardHitInfo.point;

		hitPoint = hitData.ForwardHitInfo.transform.InverseTransformPoint(hitPoint);
		var dot = Vector3.Dot(leftDir, hitPoint);

		IsMirror = dot > 0;
		if (IsMirror)
		{
			_matchBoyPart = AvatarTarget.RightHand;
		}
		else
		{
			_matchBoyPart = AvatarTarget.LeftHand;
		}
		return res;
	}
	#endregion public-method
}
