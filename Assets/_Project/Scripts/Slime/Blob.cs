using System;
using UnityEngine;
public class Blob : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    private void OnValidate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        Destroy(gameObject,3);
    }
    
}
