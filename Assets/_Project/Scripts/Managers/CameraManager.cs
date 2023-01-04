using System;
using System.Collections.Generic;
using Cinemachine;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class CameraManager : Singleton<CameraManager>
{
    private GameplayVariablesEditor m_CameraVars => GameConfig.Instance.Gameplay;
    
    [SerializeField, ReadOnly] private Camera m_MainCamera;
    [SerializeField, ReadOnly] private CinemachineVirtualCamera m_PlayerCamera;
    [SerializeField] private LevelCamera[] m_LevelCameras;
    
    public Camera MainCamera => m_MainCamera;

    [Button]
    private void setRef()
    {
        m_MainCamera = transform.FindDeepChild<Camera>("Main Camera");
        m_PlayerCamera = GameObject.Find("GameCamera").GetComponent<CinemachineVirtualCamera>();
    }

    public void ChangeCamera(Cameratype cameratype)
    {
        for (int i = 0; i < m_LevelCameras.Length; i++)
        {
            if (m_LevelCameras[i].CameraType == cameratype)
                m_LevelCameras[i].VirtualCamera.Priority = 1;
            else
                m_LevelCameras[i].VirtualCamera.Priority = 0;
        }
    }

}
