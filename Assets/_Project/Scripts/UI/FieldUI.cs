using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldUI : MonoBehaviour
{
    [SerializeField] private FieldTrigger m_FieldTrigger;
    [SerializeField] private TextMeshProUGUI m_ButtonText;
    [SerializeField] private Button m_Button;
    [SerializeField] private Image m_ButtonImage;
    [SerializeField] private GameObject m_Container;
    private Tween m_AnimationBack;
    private Tween m_AnimationText;
    private float m_AnimDuration;



    #region Editor
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_ButtonText = transform.FindDeepChild<TextMeshProUGUI>("DoneText");
        m_ButtonImage = transform.FindDeepChild<Image>("Button");
        m_Button = transform.FindDeepChild<Button>("Button");
        m_Container = transform.FindDeepChild<GameObject>("Container");
        m_FieldTrigger = FindObjectOfType<FieldTrigger>();
    }
    #endregion

     #region Init
    private void Start()
    {
      
    }

    private void OnEnable()
    {
        GameManager.onLevelStarted += onLevelStarted;
        m_Button.onClick.AddListener(ExitField);
    }

    private void OnDisable()
    {
        GameManager.onLevelStarted -= onLevelStarted;
        m_Button.onClick.RemoveListener(ExitField);
    }
    #endregion

    #region Callbacks
    private void onLevelStarted()
    {
        m_Container.SetActive(true);
    }
    #endregion

    #region Specific
    public void FadeInOut(bool state)
    {
        if (m_AnimationBack != null && m_AnimationBack.IsPlaying())
            m_AnimationBack.Complete();
        if (m_AnimationText != null && m_AnimationText.IsPlaying())
            m_AnimationText.Complete();
        
        var endValue = state ? 1 : 0;

        m_AnimationBack = m_ButtonImage.DOFade(endValue,m_AnimDuration);
        m_AnimationText = m_ButtonText.DOFade(endValue, m_AnimDuration);
    }

    private void ExitField()
    {
        FadeInOut(false);
    }
    #endregion

    #region Physics
    #endregion
}
