using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SlimeSpawnManager : MonoBehaviour
{
    [SerializeField] private List<SpawnManager> m_SpawnManagers = new List<SpawnManager>();
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
        BiomTransitionUI.OnLoadingFinished += SpawnCurrentBiomSlimes;
    }
    private void OnDisable()
    {
        BiomTransitionUI.OnLoadingFinished -= SpawnCurrentBiomSlimes;
    }
    #endregion

    #region Callbacks
    private void OnLevelLoaded()
    {

    }
    #endregion

    #region Specific

    private void SpawnCurrentBiomSlimes(int biomID, BiomTransitionUI.TransitionType type)
    {
        foreach (var manager in m_SpawnManagers)
            manager.BiomParent.gameObject.SetActive(false);
        
        m_SpawnManagers[biomID].BiomParent.gameObject.SetActive(true);
        if (type == BiomTransitionUI.TransitionType.Home)
        {
            Character.OnPlayerTeleportedHome.Invoke(biomID);
            m_SpawnManagers[biomID].StartTimer();
            return;
        }
        Character.OnTeleportedBiom.Invoke(biomID);
        CurrentBiomManager currentBiomManager = CurrentBiomManager.Instance;
        currentBiomManager.GetSlimes(m_SpawnManagers[biomID], m_SpawnManagers[biomID].SlimeList);
        currentBiomManager.SpawnSlimes();
    }

    #endregion
    
    #region Physics
    #endregion
    
}