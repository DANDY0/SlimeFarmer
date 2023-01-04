using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BagPack : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;
    [SerializeField] private DropSlimes m_DropSlimes;
    [SerializeField] private SaveBagPack m_SaveBagPack;
    public List<SlimeInfo> m_SlimesInBag;

    public int m_MaxBagCount;
    private int m_CountInBag;
    public int CountInBag { 
        get { return m_CountInBag; }
        set { m_CountInBag = value;}
    }
    public static BagStatus e_BagStatus;

    public static Action<int> OnCountChanged;
    public static Action<SlimeInfo> OnSlimeAdded;
    public static Action<SlimeInfo> OnSlimeDropped;
    private string fileName = "SaveBagPack";
    private string path;

    #region Editor
    
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_DropSlimes = GetComponent<DropSlimes>();
    }
    #endregion

    #region Init
    private void Awake()
    {
        path = Application.persistentDataPath + "/" + fileName;
        
    }
    private void Start()
    {
        Load();
        m_SlimesInBag = m_SaveBagPack.SlimesInBag;
        m_MaxBagCount = GameplayVariables.MaxBagCapacity;
        CountInBag = m_SlimesInBag.Count;
        if (m_CountInBag < m_MaxBagCount)
            e_BagStatus = BagStatus.HaveFreeSpace;
    }

    private void OnEnable()
    {
        GameManager.onLevelLoaded += OnLevelLoaded;
        OnSlimeAdded += onSlimeAdded;
        OnSlimeDropped += onSlimeDropped;
    }

    private void OnDisable()
    {
        GameManager.onLevelLoaded -= OnLevelLoaded;
        OnSlimeAdded -= onSlimeAdded;
        OnSlimeDropped -= onSlimeDropped;
    }

    #endregion
    
    #region Callbacks
    private void OnLevelLoaded(int i)
    {
     
    }

    #endregion
    
    #region Specific
    private void onSlimeAdded(SlimeInfo slimeInfo)
    {
        if(m_SlimesInBag.Count < m_MaxBagCount)
            m_SlimesInBag.Add(slimeInfo);
        m_CountInBag = m_SlimesInBag.Count;
        OnCountChanged.Invoke(m_CountInBag);
     //   if(m_CountInBag>=m_BagCapacity)
         //   FieldOfView.OnBagFull.Invoke();
         if (m_CountInBag >= m_MaxBagCount)
             e_BagStatus = BagStatus.BagIsFull;
    }

    private void onSlimeDropped(SlimeInfo slimeInfo)
    {
        if(m_SlimesInBag.Count>0)
            m_SlimesInBag.Remove(slimeInfo);
        m_CountInBag = m_SlimesInBag.Count;
        OnCountChanged.Invoke(m_CountInBag);
        e_BagStatus = BagStatus.HaveFreeSpace;
    }

    [Button]
    private void AddTestSlime(SlimeInfo info)
    {
        onSlimeAdded(info);
    }
    
    #endregion

    #region Physics

    #endregion
    private void OnApplicationQuit()
    {
        Save();
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
            Save();
    }
    private void Save()
    {
        m_SaveBagPack = new SaveBagPack();
        m_SaveBagPack.SlimesInBag = m_SlimesInBag;
        SaveManager.Instance.Save(m_SaveBagPack,path,SaveComplete,false);   
    }
    private void Load()
    {
        SaveManager.Instance.Load<SaveBagPack>(path,LoadComplete, false);
    }
    private void SaveComplete(SaveResult result, string message)
    {
        if (result == SaveResult.Error)
            Debug.LogError("Save error и + message");
    }
    private void LoadComplete(SaveBagPack saveSlimesGrid,SaveResult result, string message)
    {
        if (result == SaveResult.Error)
            Debug.LogError("Save error и + message");
        if(saveSlimesGrid ==null || saveSlimesGrid.SlimesInBag.Count<=0)
            return;
        m_SaveBagPack = saveSlimesGrid;
    }

}

public enum BagStatus
{
    HaveFreeSpace = 0,
    BagIsFull = 1,
}
