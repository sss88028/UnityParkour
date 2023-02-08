using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New Parkour Action", fileName = "ParkourAction")]
public class ParkourAction : ScriptableObject
{
    #region private-field
    [SerializeField]
    private string _stateName;
    [SerializeField]
    private float _minHeight;
    [SerializeField]
    private float _maxHeight;
    [SerializeField]
    private string _obstacleTag;

    [SerializeField]
    private bool _rotateToObstacle;

    [SerializeField]
    private string _finishStateName = "Locomotion";

    [Header("Target Matching")]
    [SerializeField]
    private bool _enableTargetMatching = true;
    [SerializeField]
    private AvatarTarget _matchBoyPart;
    [SerializeField]
    [Range(0, 1)]
    private float _matchStartTime;
    [SerializeField]
    [Range(0, 1)]
    private float _matchTargetTime;
    [SerializeField]
    private Vector3 _mathcWeight = Vector3.up;
    #endregion private-field

    #region public-property
    public string StateName 
    {
        get 
        {
            return _stateName;
        }
    }

    public bool IsRotateToObstacle
    {
        get 
        {
            return _rotateToObstacle;
        }
    }

    public string FinishStateName
    {
        get
        {
            return _finishStateName;
        }
    }

    public Quaternion TargetRotation 
    {
        get;
        private set;
    }

    public Vector3 MatchPos 
    {
        get;
        private set;
    }

    public bool EnableTargetMatching
    {
        get
        {
            return _enableTargetMatching;
        }
    }

    public AvatarTarget MatchBoyPart
    {
        get
        {
            return _matchBoyPart;
        }
    }

    public float MatchStartTime
    {
        get
        {
            return _matchStartTime;
        }
    }

    public float MatchTargetTime
    {
        get
        {
            return _matchTargetTime;
        }
    }

    public Vector3 MathcWeight
    {
        get
        {
            return _mathcWeight;
        }
    }
    #endregion public-property

    #region public-method
    public bool CheckIsPossible(CheckResult hitData, Transform player) 
    {
        if (!string.IsNullOrEmpty(_obstacleTag) && hitData.ForwardHitInfo.transform.tag != _obstacleTag) 
        {
            return false;
        }

        var height = hitData.HeightHitInfo.point.y - player.position.y;

        var isMatch = height >= _minHeight && height <= _maxHeight;
        if (!isMatch) 
        {
            return false;
        }

        if (_rotateToObstacle) 
        {
            TargetRotation = Quaternion.LookRotation(-hitData.ForwardHitInfo.normal);
        }

        if (_enableTargetMatching)
        {
            MatchPos = hitData.HeightHitInfo.point;
        }

        return isMatch;
    }
    #endregion public-method
}
