using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BiomTransitionUI : MonoBehaviour
{
    [SerializeField] private GameObject m_Container;
    [SerializeField] private Image m_Panel;
    [SerializeField] private Image[] m_Gear;
    [SerializeField] private TextMeshProUGUI m_LoadingText;

    public float m_AnimDuration;

    private Tween m_AnimationPanel;
    private Tween m_AnimationGear1;
    private Tween m_AnimationGear2;
    private Tween m_AnimationText;

    public static Action<int, TransitionType> OnLoadingFinished;
    public TransitionType e_TransitionType;
    public enum TransitionType
    {
        Biom = 0,
        Home = 1
    }

   #region Editor
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_Panel = transform.FindDeepChild<Image>("Panel");
        m_Container = transform.FindDeepChild<GameObject>("Container");
        m_LoadingText = GetComponentInChildren<TextMeshProUGUI>();
    }
    #endregion

     #region Init
    private void Start()
    {
        m_Container.SetActive(false);
    }

    private void OnEnable()
    {
        Door.OnDoorEntered += onBiomEntered;
        DoorHome.OnGoHome += onHomeEntered;
    }

    private void OnDisable()
    {
        Door.OnDoorEntered -= onBiomEntered;
        DoorHome.OnGoHome -= onHomeEntered;
    }
    #endregion

    #region Callbacks
    #endregion

    #region Specific
    #endregion
     #region Animations
    private void onBiomEntered(int index)
    {
        FadeInOut(true, index,TransitionType.Biom);
    }
    private void onHomeEntered(int index)
    {
        FadeInOut(true, index, TransitionType.Home);
    }
    
    public void FadeInOut(bool state, int index, TransitionType type)
    {
        m_Container.SetActive(true);
        var endValue = state ? 1 : 0;
       // var durationMultiplier = state ? 1 : 1;
       var durationMultiplier = 1;

        
        Sequence newSequence = DOTween.Sequence();

        m_Gear[0].transform.DORotate(new Vector3(0,0,-720), 3f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        m_Gear[1].transform.DORotate(new Vector3(0,0,720), 3f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        
        endValue = 1;
        newSequence.Append(m_Panel.DOFade(endValue, durationMultiplier * m_AnimDuration));
        newSequence.Join(m_Gear[0].DOFade(endValue, durationMultiplier * m_AnimDuration));
        newSequence.Join(m_Gear[1].DOFade(endValue, durationMultiplier * m_AnimDuration));
        newSequence.Join(m_LoadingText.DOFade(endValue, durationMultiplier * m_AnimDuration)
            .OnComplete(() => onSomethingLoad(index,type)));
        
        newSequence.AppendInterval(.4f);

        endValue = 0;
        newSequence.Append(m_Panel.DOFade(endValue, durationMultiplier * m_AnimDuration));
        newSequence.Join(m_Gear[0].DOFade(endValue, durationMultiplier * m_AnimDuration));
        newSequence.Join(m_Gear[1].DOFade(endValue, durationMultiplier * m_AnimDuration));
        newSequence.Join(m_LoadingText.DOFade(endValue, durationMultiplier * m_AnimDuration))
            .OnComplete((() => onAnimationCallback()));
        
    }

    private void onAnimationCallback()
    {
        m_Container.SetActive(false);
    }

    void onSomethingLoad(int index,TransitionType type)
    {
        if (type == TransitionType.Biom)
            OnLoadingFinished.Invoke(index, type);
        else
            OnLoadingFinished.Invoke(index,type);

    }
    #endregion
}
