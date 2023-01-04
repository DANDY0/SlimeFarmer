using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInRing : MonoBehaviour
{
    public static Action OnRingEntered;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.TryGetComponent(out Character character))
        {
            CameraManager.Instance.ChangeCamera(Cameratype.Creative02_ringEntered);
            OnRingEntered.Invoke();
        }
    }
}
