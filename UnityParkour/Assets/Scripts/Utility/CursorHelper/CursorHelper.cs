using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHelper : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
