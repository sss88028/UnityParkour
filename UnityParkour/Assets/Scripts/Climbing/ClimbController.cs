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

    private ClimbPoint _currentPoint;
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
                        _currentPoint = climbHit.transform.GetComponent<ClimbPoint>();
                        _playerController.SetControl(false);
                        JumpToLedge("IdleToHang", climbHit.transform, 0.41f, 0.54f);
                    }
                }
            }
        }
        else
        {
            var h = Mathf.Round(Input.GetAxis("Horizontal"));
            var v = Mathf.Round(Input.GetAxis("Vertical"));

            var inputDir = new Vector2(h, v);
            if (_playerController.IsInAction || inputDir == Vector2.zero) 
            {
                return;
            }

            var neighbour = _currentPoint.GetNeighbour(inputDir);
            if (neighbour == null) 
            {
                return;
            }
            if (neighbour.ConnectionType == ConnectionType.Jump && Input.GetButton("Jump")) 
            {
                _currentPoint = neighbour.Point;
                if (neighbour.Direction.y == 1)
                {
                    JumpToLedge("HopUp", _currentPoint.transform, 0.34f, 0.65f);
                }
                else if (neighbour.Direction.y == -1)
                {
                    JumpToLedge("HopDown", _currentPoint.transform, 0.31f, 0.65f);
                }
                else if (neighbour.Direction.x == 1)
                {
                    JumpToLedge("HopRight", _currentPoint.transform, 0.20f, 0.50f);
                }
                else if (neighbour.Direction.x == -1)
                {
                    JumpToLedge("HopLeft", _currentPoint.transform, 0.20f, 0.50f);
                }
            }
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
