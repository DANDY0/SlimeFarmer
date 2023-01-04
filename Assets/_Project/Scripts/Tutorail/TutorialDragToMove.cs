using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDragToMove : MonoBehaviour, Tutorial
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    [SerializeField] private GameObject m_Container;
    [SerializeField] private Image m_Pointer;
    [SerializeField] private Image m_Infinity;
    [SerializeField] private TextMeshProUGUI m_DragToMove;
    [SerializeField] private Transform m_StartPos;
    [SerializeField] private Transform m_EndPos;

    public float TimeToReset => GameplayVariables.TutorailDragTime;
    private float TimePassed;



    private Sequence TextSequence;
    private Sequence PointerSequence;

    #region Editor
    
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        
    }
    #endregion
    
    #region Init
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        TutorialManager.Instance.OnDragTutorEnabled += FadeIn;
        TutorialManager.Instance.OnDragTutorDisabled += FadeOut;
        TutorialManager.Instance.OnDragTutorDisabled += ResetTimer;
    }

    
    private void OnDisable()
    {
        TutorialManager.Instance.OnDragTutorEnabled -= FadeIn;
        TutorialManager.Instance.OnDragTutorDisabled -= FadeOut;
        TutorialManager.Instance.OnDragTutorDisabled -= ResetTimer;
    }

    #endregion
    
    #region Callbacks
    private void OnLevelLoaded()
    {
    }

    #endregion
    
    #region Specific
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !TutorialManager.Instance.isInField)
        {
            StopAllCoroutines();
            StartCoroutine(StartTimerCoroutine());
        }
    }

    public void FadeIn()
    {
        m_Container.SetActive(true);
        m_Pointer.transform.position = m_StartPos.position;
        m_Pointer.DOFade(1, .5f);
        m_DragToMove.DOFade(1, .5f);
        m_Infinity.DOFade(1, 0.5f);
        PlayAnimation();
    }

    public void PlayAnimation()
    {
        PointerSequence = DOTween.Sequence();
        
        PointerSequence.Append(m_Pointer.transform.DOMove(m_EndPos.position, 1.5f));
       // PointerSequence.Append(m_Pointer.transform.DOMove(m_StartPos.position, 1f));
       // PointerSequence.Append(m_Pointer.transform.DOMove(m_EndPos.position, 1f));
        PointerSequence.SetLoops(-1,LoopType.Yoyo);
        PointerSequence.SetEase(Ease.InOutSine);
        
        TextSequence = DOTween.Sequence();
        TextSequence.Append(m_DragToMove.transform.DOScale(Vector3.one*1f, .3f));
        TextSequence.Append(m_DragToMove.transform.DOScale(Vector3.one*1.2f, .6f));
        TextSequence.Append(m_DragToMove.transform.DOScale(Vector3.one*1f, .3f));
        //textSequence.Append(m_DragToMove.transform.DOScale(Vector3.one*1.2f, 1f));
         
        TextSequence.SetLoops(-1);
        
    }

    public void FadeOut()
    {
        m_Pointer.DOFade(0, .5f) 
            .OnComplete(()=> PointerSequence?.Kill());
        m_DragToMove.DOFade(0, .5f)
            .OnComplete(()=> TextSequence?.Kill());
        m_Infinity.DOFade(0, 0.5f)
            .OnComplete(()=> m_Container.SetActive(false));
        
    }

    private void ResetTimer()
    {
        TimePassed = 0;
    }

    private IEnumerator StartTimerCoroutine()
    {
        TimePassed = 0;
        while (TimePassed<TimeToReset)
        {
            TimePassed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        if (!Input.GetMouseButton(0) )
        {
            TutorialManager.Instance.OnDragTutorEnabled.Invoke();
            yield break;
        }

    }
    #endregion
    
    
}
