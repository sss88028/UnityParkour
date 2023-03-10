using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClimbPoint : MonoBehaviour
{
    #region private-field
    [SerializeField]
    private List<Neighbour> _neighbours;

    [SerializeField]
    private bool _isMountPoint;
    #endregion private-field

    #region public-property
    public bool IsMountPoint => _isMountPoint;
    #endregion public-property

    #region public-method
    public Neighbour GetNeighbour(Vector2 direction) 
    {
        var neighbour = _neighbours.FirstOrDefault((n) =>
        {
            if (direction.y != 0)
            {
                if (n.Direction.y == direction.y) 
                {
                    return true;
                }
            }
            if (direction.x != 0)
            {
                if (n.Direction.x == direction.x)
                {
                    return true;
                }
            }
            return false;
        }
        );
        return neighbour;
    }
    #endregion public-method

    #region MonoBehaviour-method
    private void Awake()
    {
        AutoConnectNeigbour();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        foreach (var neighbour in _neighbours)
        {
            if (neighbour.Point != null) 
            {
                Debug.DrawLine(transform.position, neighbour.Point.transform.position, neighbour.IsTwoWay ? Color.green : Color.gray);
            }
        }
    }
    #endregion MonoBehaviour-method

    #region private-method
    private void AutoConnectNeigbour() 
    {
        var neighbours = _neighbours.Where(n => n.IsTwoWay);
        foreach (var neighbour in neighbours) 
        {
            neighbour.Point.CreateConnect(new Neighbour() 
            {
                Point = this,
                Direction = -neighbour.Direction,
                ConnectionType = neighbour.ConnectionType,
                IsTwoWay = neighbour.IsTwoWay,
            });
        }
    }

    private void CreateConnect(Neighbour neighbour) 
    {
        _neighbours.Add(neighbour);
    }
    #endregion private-method
}

[Serializable]
public class Neighbour 
{
    public ClimbPoint Point;
    public Vector2 Direction;
    public ConnectionType ConnectionType;
    public bool IsTwoWay = true;
}

public enum ConnectionType 
{
    Jump,
    Move,
}