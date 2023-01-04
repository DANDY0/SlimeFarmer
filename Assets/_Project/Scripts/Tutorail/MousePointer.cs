using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{ 
    //[SerializeField] private ZoomToggleButton m_ZoomToggleButton; 
    [SerializeField] private SpriteRenderer m_BrushSprite;
    [SerializeField] private GameObject m_BrushContainer;
    
    public Vector3 m_BrushOffset;
    private bool m_CanScratch;

    public static Action<bool> OnCanShowBrush;
    private void OnEnable()
    {
        OnCanShowBrush += ShowBrush;
        ShowBrush(false);
    }

    public void OnDisable()
    {
        OnCanShowBrush -= ShowBrush;
    }
    private void Update()
    {
        MoveBrush();
    }

    private void OnMouseDown()
    {
        ShowBrush(true);
    }

    private void OnMouseUp()
    {
        ShowBrush(false);
    }
    public void MoveBrush()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + m_BrushOffset);
        mousePos .z = 0; 
        var speed = 500; 
        transform.position = Vector3.Lerp(transform.position, mousePos +new Vector3(0,0,-2), speed * Time.deltaTime);
    }

    private void ChangeSize(float scaleFactor)
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor);
    }

    public void ShowBrush(bool state)
    {
      
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + m_BrushOffset);
        mousePos .z = 0;
        transform.position = mousePos +new Vector3(0,0,-2);
        m_CanScratch = state;
        m_BrushSprite.enabled = state;
    }

    private void CanShowBrush(bool state)
    {
        m_BrushContainer.SetActive(state);
    }
    
    
}


