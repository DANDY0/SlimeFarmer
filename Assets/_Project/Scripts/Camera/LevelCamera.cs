using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    [SerializeField] private Cameratype m_Cameratype;
    public Cameratype CameraType => m_Cameratype;
    public CinemachineVirtualCamera VirtualCamera => m_VirtualCamera;

    [SerializeField] private CinemachineVirtualCamera m_VirtualCamera;

    private void OnValidate()
    {
        m_VirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
}

public enum Cameratype
{
    PlayerCamera = 0,
    FieldCamera00 = 1,
    FieldCamera01 = 2,
    FieldCamera02 = 3,
    FieldCamera03 = 4,
    FieldCamera04 = 5,
    FieldCamera05 = 6,
    FieldCamera06 = 7,
    FieldCamera07 = 8,
    FieldCamera08 = 9,
    FieldCamera09 = 10,
    Creative01 = 11,
    Creative01_merge1 = 12,
    Creative01_merge2 = 13,
    Creative02_ringEntered = 14,
    Creative02_ringEaten = 15,
    Creative02_ringCry = 16,
    Creative03_dream = 17,
    Creative03_cryMoney = 18,
    
}
