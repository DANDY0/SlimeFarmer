using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class DreamAnimations : MonoBehaviour
{
    [SerializeField] private GameObject m_Dream;
    [SerializeField] private GameObject m_Idea;
    [SerializeField] private Transform dreamStartPos;
    [SerializeField] private Vector3 dreamEndScale;
    [SerializeField] private Transform ideaStartPos;
    [SerializeField] private Vector3 ideaEndScale;

    private void Start()
    {
        this.WaitRealSecond(4f,ShowDream);
    }
    
    [Button]
    private void ShowDream()
    {
        Sequence dreamSequence = DOTween.Sequence();
        dreamSequence.Append(m_Dream.transform.DOScale(dreamEndScale,.5f));
        dreamSequence.AppendInterval(1f);
        dreamSequence.Append(m_Dream.transform.DOScale(Vector3.zero, 0.5f));
        dreamSequence.AppendCallback(()=> {
            m_Idea.SetActive(true);
            m_Dream.SetActive(false);});
        dreamSequence.Append(m_Idea.transform.DOScale(ideaEndScale, 0.5f));
        dreamSequence.AppendInterval(1f);
        dreamSequence.Append(m_Idea.transform.DOScale(Vector3.zero, 0.5f))
            .OnComplete(()=> m_Idea.SetActive(false));
        dreamSequence.AppendCallback((() =>
        {
            CameraManager.Instance.ChangeCamera(Cameratype.PlayerCamera);
            SpawnSlimes.Instance.SpawnSlimesMethod();
        }));
    }
    private void ShowIdea()
    {
        
    }

}
