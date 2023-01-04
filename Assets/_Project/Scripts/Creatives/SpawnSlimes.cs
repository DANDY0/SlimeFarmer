using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.Setup;
using UnityEngine;

public class SpawnSlimes : Singleton<SpawnSlimes>
{
    [SerializeField] private GameObject[] m_Slimes;

    public void SpawnSlimesMethod()
    {
        foreach (var slime in m_Slimes)
            slime.SetActive(true);
    }
}
