using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class OffScreenPlayerArrowManager : Singleton<OffScreenPlayerArrowManager>
{
    [SerializeField] private Canvas m_Canvas;
    [SerializeField] private RectTransform m_Arrow;
    [SerializeField] private Image m_ArrowDisplay;
    

    private Camera m_MainCamera;

    private Vector2 m_DummyOnScreenPos;
    private Vector2 m_DummyArrowPos;

    #region Unity Loop
    public override void Start()
    {
        base.Start();

        if (m_MainCamera == null)
            m_MainCamera = CameraManager.Instance.MainCamera;
    }

    private void OnEnable()
    {

    }

    public override void OnDisable()
    {
        base.OnDisable();
    }
    #endregion

    #region Events
    public void OnReset()
    {
        m_Arrow.gameObject.SetActive(false);
    }
    #endregion

    public void ShowArrowIfPlayerOffscreen(Transform i_PlayerTransform, float offset, Vector3 size)
    {
        m_DummyOnScreenPos = m_MainCamera.WorldToScreenPoint(i_PlayerTransform.position);

        if ((m_DummyOnScreenPos.x < 0 || m_DummyOnScreenPos.x > Screen.width || m_DummyOnScreenPos.y < 0 || m_DummyOnScreenPos.y > Screen.height))
        {
            m_Arrow.gameObject.SetActive(true);
            
            m_DummyArrowPos = m_DummyOnScreenPos;
            m_DummyArrowPos.Set
                (
                    Mathf.Clamp(m_DummyArrowPos.x, 0, Screen.width),
                    Mathf.Clamp(m_DummyArrowPos.y, 0 + size.y + offset, Screen.height - size.y)
                );


            m_Arrow.anchoredPosition = m_DummyArrowPos / m_Canvas.scaleFactor;
            m_Arrow.transform.up = (m_DummyOnScreenPos - m_DummyArrowPos).normalized;
        }
        else
        {
            m_Arrow.gameObject.SetActive(false);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        m_ArrowDisplay.sprite = sprite;
    }

    public void Hide()
    {
        m_Arrow.gameObject.SetActive(false);
    }
}
