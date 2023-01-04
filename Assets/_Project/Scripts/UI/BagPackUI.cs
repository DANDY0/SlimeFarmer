using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BagPackUI : MonoBehaviour
{
    [SerializeField] private BagPack m_BagPack;
    [SerializeField] private TextMeshProUGUI m_BagCapacityText;
    [SerializeField] private GameObject m_Container;
    [SerializeField] private int m_MaxCapacity;
    [SerializeField] private int m_CurrentValue;
    private Camera m_Camera;
    //[SerializeField] private Color m_FullColor;
    #region Editor
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_BagCapacityText = transform.FindDeepChild<TextMeshProUGUI>("Capacity");
        m_Container = transform.FindDeepChild<GameObject>("Container");
        m_BagPack = FindObjectOfType<BagPack>();
    }
    #endregion

     #region Init
    private void Start()
    {
        m_Camera = Camera.main;
    }

    private void OnEnable()
    {
        GameManager.onLevelStarted += onLevelStarted;
        BagPack.OnCountChanged += onCountChanged;
    }

    private void OnDisable()
    {
        GameManager.onLevelStarted -= onLevelStarted;
        BagPack.OnCountChanged -= onCountChanged;
    }
    #endregion

    #region Callbacks
    private void onLevelStarted()
    {
        m_MaxCapacity = m_BagPack.m_MaxBagCount;
        m_CurrentValue = m_BagPack.CountInBag;
        m_Container.SetActive(true);
    }
    #endregion

    #region Specific
    private void LateUpdate()
    {
        m_BagCapacityText.text = m_CurrentValue + "/" + m_MaxCapacity;
        transform.rotation = Quaternion.LookRotation(transform.position - m_Camera.transform.position);
        //m_Slider.value = m_TargetValue;
    }

    private void onCountChanged(int slimesCount)
    {
        m_CurrentValue = slimesCount;
    }
    #endregion
    public void offText()
    {
        m_BagCapacityText.enabled = false;
    }

    #region Physics
    #endregion
}
