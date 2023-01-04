using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{

    [SerializeField] private Button m_Button;

    [Button]
    private void setRefs()
    {
        m_Button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        m_Button.onClick.AddListener(ButtonExit);
    }
    private void OnDisable()
    {
        m_Button.onClick.RemoveListener(ButtonExit);
    }
    private void ButtonExit()
    {
        Application.Quit();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
}
