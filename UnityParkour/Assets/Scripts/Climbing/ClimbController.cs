using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbController : MonoBehaviour
{
    #region private-field
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
        if (!Input.GetButton("Jump"))
        {
            return;
        }

        if (_environmentScanner == null)
        {
            return;
        }
        var isHit = _environmentScanner.ClimbLedgeCheck(transform.forward, out var climbHit);
    }
    #endregion private-method
}
