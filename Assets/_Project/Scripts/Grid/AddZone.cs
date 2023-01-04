using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MergeSlime slime))
            slime.SetAddColor();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MergeSlime slime))
            slime.SetDefaultFromAdd();
    }
}
