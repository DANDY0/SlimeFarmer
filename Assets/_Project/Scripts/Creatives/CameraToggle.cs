using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            CameraManager.Instance.ChangeCamera(Cameratype.Creative01_merge1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            CameraManager.Instance.ChangeCamera(Cameratype.Creative01_merge2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            CameraManager.Instance.ChangeCamera(Cameratype.FieldCamera00);

    }

    private void SetTransform()
    {
        
    }
}
