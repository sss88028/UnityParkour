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
    private bool _rotateToObstacle;
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

    public Quaternion TargetRotation 
    {
        get;
        private set;
    }
    #endregion public-property

    #region public-method
    public bool CheckIsPossible(RaycastHit hitInfo, RaycastHit heightInfo, Transform player) 
    {
        var height = heightInfo.point.y - player.position.y;

        var isMatch = height >= _minHeight && height <= _maxHeight;
        if (!isMatch) 
        {
            return false;
        }

        if (_rotateToObstacle) 
        {
            TargetRotation = Quaternion.LookRotation(-hitInfo.normal);
        }

        return isMatch;
    }
    #endregion public-method
}
