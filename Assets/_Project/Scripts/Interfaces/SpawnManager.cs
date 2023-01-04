using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SpawnManager: MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    public int BiomID;
    public SlimeType e_SlimesType;
    public int SlimesCount;
    public Slime PrefabVariant;
    public List<Slime> SlimeList;
    public Transform SlimesContainer;
    public Transform SpawnPosContainer;
    public List<Transform> SpawnPositions;
    public Transform PlayerSpawnPos;
    public Transform BiomParent;

    public float TimeToReset => GameplayVariables.TimeToResetBiom;
    public float TimePassed;
    public bool NeedToRespawn;

    [Button]
    private void SetRefs()
    {
        SlimeList.Clear();
        for (int i = 0; i < SlimesCount; i++)
            SlimeList.Add(PrefabVariant);
        // SpawnPosContainer = transform.FindDeepChild("SpawnPosContainer");
        SpawnPositions = SpawnPosContainer.GetComponentsInChildren<Transform>().ToList();
        SpawnPositions.RemoveAt(0);
    }

    public void StartTimer()
    {
        StopAllCoroutines();
        StartCoroutine(StartTimerCoroutine());
    }
    public void ResetTimer()
    {
        StopAllCoroutines();
        TimePassed = 0;
       // NeedToRespawn = false;
    }
    private IEnumerator StartTimerCoroutine()
    {
        TimePassed = 0;
        NeedToRespawn = false;
        while (TimePassed<TimeToReset)
        {
            TimePassed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        NeedToRespawn = true;
    }
}
