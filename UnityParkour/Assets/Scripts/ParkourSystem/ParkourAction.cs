using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New Parkour Action", fileName = "ParkourAction")]
public class ParkourAction : ScriptableObject
{
    #region protected-field
    [SerializeField]
    protected string _stateName;
    [SerializeField]
    protected float _minHeight;
    [SerializeField]
    protected float _maxHeight;
    [SerializeField]
    protected string _obstacleTag;

    [SerializeField]
    protected bool _rotateToObstacle;

    [SerializeField]
    protected string _finishStateName = "Locomotion";

    [Header("Target Matching")]
    [SerializeField]
    protected bool _enableTargetMatching = true;
    [SerializeField]
    protected AvatarTarget _matchBoyPart;
    [SerializeField]
    [Range(0, 1)]
    protected float _matchStartTime;
    [SerializeField]
    [Range(0, 1)]
    protected float _matchTargetTime;
    [SerializeField]
    protected Vector3 _mathcWeight = Vector3.up;
    #endregion protected-field

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
        protected set;
    }

    public Vector3 MatchPos 
    {
        get;
        protected set;
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

    public bool IsMirror 
    {
        get;
        protected set;
    }
    #endregion public-property

    #region public-method
    public virtual bool CheckIsPossible(ObstacleHitData hitData, Transform player) 
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
