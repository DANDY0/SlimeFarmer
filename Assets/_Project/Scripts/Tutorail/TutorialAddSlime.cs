using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAddSlime : MonoBehaviour, Tutorial
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    [SerializeField] private GameObject m_Container;
    [SerializeField] private Image m_Pointer;
    [SerializeField] private TextMeshProUGUI m_AddToBag;
    [SerializeField] private Transform m_StartPos;
    [SerializeField] private Transform m_EndPos;

    public float TimeToReset => GameplayVariables.TutorailDragTime;
    private float TimePassed;

    private Sequence TextSequence;
    private Sequence PointerSequence;

    private void OnEnable()
    {
        TutorialManager.Instance.OnAddTutorEnabled += FadeIn;
        TutorialManager.Instance.OnAddTutorDisabled += FadeOut;
    }
    private void OnDisable()
    {
        TutorialManager.Instance.OnAddTutorEnabled -= FadeIn;
        TutorialManager.Instance.OnAddTutorDisabled -= FadeOut;
    }

    public void FadeIn()
    {
        m_Container.SetActive(true);
        m_Pointer.transform.position = m_StartPos.position;
        m_Pointer.DOFade(1, .5f);
        m_AddToBag.DOFade(1, .5f);
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        PointerSequence = DOTween.Sequence();
        
        PointerSequence.Append(m_Pointer.transform.DOMove(m_EndPos.position, 1.5f));
        PointerSequence.SetLoops(-1,LoopType.Yoyo);
        PointerSequence.SetEase(Ease.InOutSine);
        
        TextSequence = DOTween.Sequence();
        TextSequence.Append(m_AddToBag.transform.DOScale(Vector3.one*1f, .3f));
        TextSequence.Append(m_AddToBag.transform.DOScale(Vector3.one*1.2f, .6f));
        TextSequence.Append(m_AddToBag.transform.DOScale(Vector3.one*1f, .3f));

        TextSequence.SetLoops(-1);
        
    }

    public void FadeOut()
    {
        m_Pointer.DOFade(0, .5f);
        m_AddToBag.DOFade(0, .5f)
            .OnComplete(KillAnims);
    }
    private void KillAnims()
    {
        PointerSequence?.Kill();
        TextSequence?.Kill();
        m_Container.SetActive(false);
    }


}

