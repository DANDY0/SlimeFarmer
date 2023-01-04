using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using  DG.Tweening;
using Sirenix.OdinInspector;
public class SlimeCatchBar : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;
    [SerializeField] private Slider m_Slider;

    [SerializeField] private Image m_Background;
    [SerializeField] private Image m_Fill;

    private Camera m_Camera;
    public float m_AnimDuration;
    private float m_TargetValue;

    private Tween m_AnimationBack;
    private Tween m_AnimationFill;

    #region Editor
    private void OnValidate()
    {
        SetRefs();
    }
    [Button]
    private void SetRefs()
    {
        m_Slider = transform.FindDeepChild<Slider>("Slider");
        m_Background = transform.FindDeepChild<Image>("Background");
        m_Fill = transform.FindDeepChild<Image>("Fill");
    }
    #endregion
    
    #region CallBacks
    private void Start()
    {
        m_Camera = Camera.main;
        m_Slider.maxValue = GameplayVariables.MaxCatchTime;
        m_TargetValue = m_Slider.maxValue;
        m_AnimDuration = GameplayVariables.SlimeBarAnimDuration;
        FadeInOut(false);
    }
    private void OnEnable()
    {
      //  FieldOfView.OnSlimeCatching += FadeInOut;
    }
    private void OnDisable()
    {
        //FieldOfView.OnSlimeCatching -= FadeInOut;
    }
   
  #endregion

    #region Specific
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - m_Camera.transform.position);
        m_Slider.value = m_TargetValue;
    }

    public void UpdateCatchBar(float currentCatchValue)
    {
        m_TargetValue = currentCatchValue;
    }
  #endregion

    #region Animations
    public void FadeInOut(bool state)
    {
        if (m_AnimationBack != null && m_AnimationBack.IsPlaying())
            m_AnimationBack.Complete();
        if (m_AnimationFill != null && m_AnimationFill.IsPlaying())
            m_AnimationFill.Complete();
        
        var endValue = state ? 1 : 0;
        var durationMultiplier = state ? 1 : 5;
        
        m_AnimationBack = m_Background.DOFade(endValue,durationMultiplier * m_AnimDuration);
        m_AnimationFill = m_Fill.DOFade(endValue, durationMultiplier * m_AnimDuration);
    }

    #endregion
  

 
}
