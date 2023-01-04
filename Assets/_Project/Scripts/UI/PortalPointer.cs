using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PortalPointer : MonoBehaviour
{
    [SerializeField] private Transform[] m_Targets;
    [SerializeField, ReadOnly] private RectTransform m_Pointer;
    [SerializeField, ReadOnly] private Image m_HomeIcon;
    [SerializeField, ReadOnly] private CanvasGroup m_CanvasGroup;
    [Title("")]
    [SerializeField] private Canvas m_Canvas;

    private Vector2 m_DummyOnScreenPos;
    private Vector2 m_DummyArrowPos;

    [SerializeField] private bool m_IsNeedShowBar;
    private Transform m_CurrentTarget;
    [SerializeField] private Character m_Player;

    #region Editor
    [Button]
    protected virtual void SetRefs()
    {
        m_Pointer = GetComponent<RectTransform>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_HomeIcon = transform.FindDeepChild<Image>("Icon");
        m_Player = FindObjectOfType<Character>();
    }
    private void OnValidate()
    {
        SetRefs();
    }
    #endregion

    #region Init
    private void OnEnable()
    {
        GameManager.onLevelStarted += onLevelStarted;
        Door.OnDoorEntered += enablePointer;
        DoorHome.OnGoHome += disablePointer;
        // BeerStack.onValueChanged += onBeerValueChanged;
        //BeerStack.onAddMug += onAddMug;
    }

    private void OnDisable()
    {
        GameManager.onLevelStarted -= onLevelStarted;
        Door.OnDoorEntered -= enablePointer;
        DoorHome.OnGoHome -= disablePointer;
        //BeerStack.onValueChanged -= onBeerValueChanged;
        //BeerStack.onAddMug -= onAddMug;
    }
    #endregion

    private void onLevelStarted()
    {
        m_CurrentTarget = m_Targets[0];
        m_IsNeedShowBar = false;
    }
    private void disablePointer(int _)
    {
        m_IsNeedShowBar = false;
    }
    private void enablePointer(int id)
    {
        this.WaitSecond(1f,() => {
            m_CurrentTarget = m_Targets[id];
            m_IsNeedShowBar = true;
        });
       
    }

    #region UnityLoop
    private void Update()
    {
        if (m_IsNeedShowBar == false)
            return;

        if (m_CanvasGroup.alpha == 0)
            return;

        m_HomeIcon.rectTransform.rotation = Quaternion.identity;
    }

    private void LateUpdate()
    {
        if (m_IsNeedShowBar == false)
        {
            if (m_CanvasGroup.alpha != 0)
                m_CanvasGroup.alpha = 0;

            return;
        }

        showArrowIfPlayerOffscreen(m_CurrentTarget, m_Pointer.sizeDelta / 2);
    }

    private void showArrowIfPlayerOffscreen(Transform target, Vector3 size)
    {

        if (Vector3.Distance(target.position, m_Player.transform.position) > 5f)
        {
            var direction = (target.position - m_Player.transform.position).normalized;
            m_DummyOnScreenPos = m_MainCamera.WorldToScreenPoint(m_Player.transform.position + direction * 8f);
        }
        else
        {
            m_DummyOnScreenPos = m_MainCamera.WorldToScreenPoint(target.position);
        }

        if ((m_DummyOnScreenPos.x < 0 || m_DummyOnScreenPos.x > Screen.width || m_DummyOnScreenPos.y < 0 || m_DummyOnScreenPos.y > Screen.height))
        {
            if (m_CanvasGroup.alpha != 1)
                m_CanvasGroup.alpha = 1;

            m_DummyArrowPos = m_DummyOnScreenPos;
            m_DummyArrowPos.Set
                (
                    Mathf.Clamp(m_DummyArrowPos.x, 0, Screen.width),
                    Mathf.Clamp(m_DummyArrowPos.y, size.y, Screen.height - size.y)
                );

            if (isTopOrDown(size.y))
                m_DummyArrowPos.x = Mathf.Clamp(m_DummyArrowPos.x, size.x, Screen.width - size.x);

            m_Pointer.anchoredPosition = m_DummyArrowPos / m_Canvas.scaleFactor;
            m_Pointer.transform.up = (m_DummyOnScreenPos - m_DummyArrowPos).normalized;
        }
        else
        {
            if (m_CanvasGroup.alpha != 0)
                m_CanvasGroup.alpha = 0;
        }
    }
    #endregion

    private bool isTopOrDown(float size) => (m_DummyArrowPos.y == size || m_DummyArrowPos.y == Screen.height - size);

    private Camera m_MainCamera => CameraManager.Instance.MainCamera;

}
