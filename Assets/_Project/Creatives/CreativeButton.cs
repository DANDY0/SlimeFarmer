using DG.Tweening;
using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreativeButton : MonoBehaviour
{
    [SerializeField] private RectTransform m_MovingImage;

    public static event Action onClick;

    private float m_StartY;

    private void Awake()
    {
        m_StartY = m_MovingImage.anchoredPosition.y;

        GetComponent<Button>().Set(() =>
        {
            m_MovingImage.DOAnchorPosY(-8, 0.25f).OnComplete(() =>
            {
                onClick?.Invoke();
                m_MovingImage.DOAnchorPosY(m_StartY, 0.25f).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(0.25f, () => transform.parent.gameObject.SetActive(false));
                });
            });
        });  
    }
}
