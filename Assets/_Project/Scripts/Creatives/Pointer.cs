using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Pointer : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private Image image;
    public static Action<bool> onShowPointer;
    private void OnEnable()
    {
        onShowPointer += changePointerState;
        
    }
    private void OnDisable()
    {
        onShowPointer += changePointerState;
    }
    void Update()
    {
        var screenPoint =(Input.mousePosition);
        screenPoint.z = 10.0f; //distance of the plane from the camera
        parent.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    private void changePointerState(bool state)
    {
        image.enabled = state;
    }
}
