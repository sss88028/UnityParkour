using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    #region private-field
    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private EnvironmentScanner _environmentScanner;
    #endregion private-field

    #region MonoBehaviour-method
    private void Update()
    {
        CheckClimb();
    }
    #endregion MonoBehaviour-method

    #region private-method
    private void CheckClimb()
    {
        if (!_playerController.IsHanging)
        {
            if (Input.GetButton("Jump") && !_playerController.IsInAction)
            {
                if (_environmentScanner != null)
                {
                    var isHit = _environmentScanner.ClimbLedgeCheck(transform.forward, out var climbHit);

                    if (isHit)
                    {
                        _playerController.SetControl(false);
                        JumpToLedge("IdleToHang", climbHit.transform, 0.41f, 0.54f);
                    }
                }
            }
        }
        else 
        {
        }
    }

    private async void JumpToLedge(string anim, Transform ledge, float matchStartTime, float matchTargetTime) 
    {
        var matchPatams = new MatchTargetParams()
        {
            MatchPos = GetHandPos(ledge),
            MatchBoyPart = AvatarTarget.RightHand,
            MatchStartTime = matchStartTime,
            MatchTargetTime = matchTargetTime,
            PosWeight = Vector3.one,
        };
        var targetRot = Quaternion.LookRotation(-ledge.forward);
        await _playerController.DoAction(anim, "HangingIdle", matchPatams, targetRot, true);

        _playerController.IsHanging = true;
    }

    private Vector3 GetHandPos(Transform ledge) 
    {
        return ledge.position + ledge.forward * 0.1f + Vector3.up * 0.1f - ledge.right * 0.25f;
    }
    #endregion private-method
}
