using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class CurrentBiomManager : Singleton<CurrentBiomManager>
{
    
    [SerializeField] private List<Slime> m_CurrentBiomSlimes;
    [SerializeField] private SpawnManager m_CurrentSpawnManager;
    [SerializeField] private List<Transform> m_CurrentSpawnPositions;
    [SerializeField] private Transform m_CurrentSlimesContainer;
    [SerializeField] private float m_SpawnRange;
    

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
    private void OnEnable()
    {
        FieldOfView.OnSlimeEnterField += onSlimeActionWithField;
        FieldOfView.OnSlimeExitField += onSlimeActionWithField;
    
    }
    private void OnDisable()
    {
        FieldOfView.OnSlimeEnterField -= onSlimeActionWithField;
        FieldOfView.OnSlimeExitField -= onSlimeActionWithField;

    }
    #endregion

    #region Callbacks
    private void OnLevelLoaded()
    {

    }
    #endregion
    
    #region Specific

    public void GetSlimes(SpawnManager spawnManager,List<Slime> slimes)
    {
        m_CurrentSpawnManager = spawnManager;
        m_CurrentBiomSlimes = slimes;
        m_CurrentSlimesContainer = m_CurrentSpawnManager.SlimesContainer;
        m_CurrentSpawnPositions = spawnManager.SpawnPositions;
    }

    public void SpawnSlimes()
    {
        if(m_CurrentSpawnManager==null)
            return;
        
        Character.OnPlayerTeleportedBiom.Invoke(m_CurrentSpawnManager.PlayerSpawnPos,m_CurrentSpawnManager.BiomParent);
        m_CurrentSpawnManager.ResetTimer();
        if (m_CurrentSpawnManager.SlimesContainer.childCount==0)
            m_CurrentSpawnManager.NeedToRespawn = true;
        if (!m_CurrentSpawnManager.NeedToRespawn)
            return;
        
        foreach (Transform child in m_CurrentSlimesContainer) 
            Destroy(child.gameObject);
        
        m_CurrentSpawnPositions.Shuffle();

        for(int i= 0; i< m_CurrentBiomSlimes.Count; i++)
        { 
            var newSlime = Instantiate(m_CurrentBiomSlimes[i], m_CurrentSlimesContainer);
            newSlime.transform.localPosition = m_CurrentSpawnPositions[i].localPosition;
            newSlime.InitSlime(calculateSlimeInfo());
        }
        
        
    }
    
    private SlimeInfo calculateSlimeInfo()
    {
        var info = new SlimeInfo(); 
        var randomValue = Random.Range(0,101);
        
        if (randomValue < 50)
            info.SlimeMainIndex = 0;
        else if(randomValue < 80)
            info.SlimeMainIndex = 1;
        else if(randomValue < 100)
            info.SlimeMainIndex = 2;
        info.e_SlimeMainType = m_CurrentSpawnManager.e_SlimesType;
        return info;
    }
    
    private void onSlimeActionWithField(Slime slime, bool state)
    {
        slime.StopAllCoroutines();
        slime.OnCatchStateUpdated(state);
    }
    
    #endregion

    #region Physics
    #endregion
}
