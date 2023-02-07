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
    #endregion private-field

    #region public-property
    public string StateName 
    {
        get 
        {
            return _stateName;
        }
    }
    #endregion public-property

    #region public-method
    public bool CheckIsPossible(RaycastHit heightInfo, Transform player) 
    {
        var height = heightInfo.point.y - player.position.y;

        var isMatch = height >= _minHeight && height <= _maxHeight;
        return isMatch;
    }
    #endregion public-method
}
