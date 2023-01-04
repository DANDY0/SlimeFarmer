using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MergeSlime slime))
            slime.SetDeleteColor();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MergeSlime slime))
            slime.SetDefaultFromDelete();
    }
}
