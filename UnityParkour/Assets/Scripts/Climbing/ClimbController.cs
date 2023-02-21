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
                    var handOffset = new Vector3(0.25f, 0.08f, 0.15f);
                    JumpToLedge("HopUp", _currentPoint.transform, 0.34f, 0.65f, handOffset: handOffset);
                }
                else if (neighbour.Direction.y == -1)
                {
                    var handOffset = new Vector3(0.25f, 0.1f, 0.13f);
                    JumpToLedge("HopDown", _currentPoint.transform, 0.31f, 0.65f, handOffset: handOffset);
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
            else if (neighbour.ConnectionType == ConnectionType.Move)
            {
                _currentPoint = neighbour.Point;
                var handOffset = new Vector3(0.25f, 0.05f, 0.1f);
                if (neighbour.Direction.x == 1)
                {
                    JumpToLedge("ShimmyRight", _currentPoint.transform, 0f, 0.38f, handOffset : handOffset);
                }
                else if (neighbour.Direction.x == -1)
                {
                    JumpToLedge("ShimmyLeft", _currentPoint.transform, 0f, 0.38f, AvatarTarget.LeftHand, handOffset);
                }
            }
        }
    }

    private async void JumpToLedge(string anim, Transform ledge, float matchStartTime, float matchTargetTime, 
        AvatarTarget hand = AvatarTarget.RightHand,
        Vector3? handOffset = null) 
    {
        var matchPatams = new MatchTargetParams()
        {
            MatchPos = GetHandPos(ledge, handOffset, hand),
            MatchBoyPart = hand,
            MatchStartTime = matchStartTime,
            MatchTargetTime = matchTargetTime,
            PosWeight = Vector3.one,
        };
        var targetRot = Quaternion.LookRotation(-ledge.forward);
        await _playerController.DoAction(anim, "HangingIdle", matchPatams, targetRot, true);

        _playerController.IsHanging = true;
    }

    private Vector3 GetHandPos(Transform ledge, Vector3? handOffset, AvatarTarget hand = AvatarTarget.RightHand) 
    {
        var offest = handOffset.HasValue ? handOffset.Value : new Vector3(0.25f, 0.1f, 0.1f);

        var handDir = hand == AvatarTarget.RightHand ? ledge.right : -ledge.right;
        return ledge.position + ledge.forward * offest.z + Vector3.up * offest.y - handDir * offest.x;
    }
    #endregion private-method
}
