using System;
using System.Collections.Generic;
using UnityEngine;
public class IncomeManager : MonoBehaviour
{
    [SerializeField] private List<SlimeInfo> m_DiscoveredSlimes;
    [SerializeField] private SaveDiscoveredSlimes m_SaveSaveDiscoveredSlimes;
    [SerializeField] private int m_NewSlimeReward;
    [SerializeField] private AudioClip m_UnlockSound;
    [SerializeField] private AudioClip m_CoinSound;
    public int SlimeTypesCount;

    public static Action<SlimeInfo, MergeSlime> onSlimeMerged;
    const string fileName = "SaveDiscoveredSlimes";
    string path;
    
    private void Awake()
    {
        path = Application.persistentDataPath + "/" + fileName;

        Load();
        
    }

    private void OnEnable()
    {
        onSlimeMerged += CheckSlimeUnique;
    }
    private void OnDisable()
    {
        onSlimeMerged -= CheckSlimeUnique;
    }
    private void CheckSlimeUnique(SlimeInfo info, MergeSlime mergeSlime)
    {
        var newSlime = new SlimeInfo(info);
        
        if (m_DiscoveredSlimes.Count == 0 && info.isMerged)
        {
            m_DiscoveredSlimes.Add(newSlime);
            CopySlimeManager.Instance.MergeSlimeCopy(mergeSlime);
            m_CollectableManager.GetWallet(eCollectable.Coin).Add(m_NewSlimeReward);
            m_CollectableManager.GetWallet(eCollectable.UniqueSlimes).Add(1);
            AudioManager.Instance.PlayUnlockSound();
            SoundManager.Instance.PlaySFX(m_CoinSound);
            SoundManager.Instance.PlaySFX(m_UnlockSound);
            mergeSlime.EnableNewAnimation();
            return;
        }

        if (!m_DiscoveredSlimes.Contains(newSlime) && info.isMerged)
        {
            m_DiscoveredSlimes.Add(newSlime);
            CopySlimeManager.Instance.MergeSlimeCopy(mergeSlime);
            m_CollectableManager.GetWallet(eCollectable.UniqueSlimes).Add(1);
            m_CollectableManager.GetWallet(eCollectable.Coin).Add(m_NewSlimeReward);
            AudioManager.Instance.PlayUnlockSound();
            SoundManager.Instance.PlaySFX(m_CoinSound);
            SoundManager.Instance.PlaySFX(m_UnlockSound);
            mergeSlime.EnableNewAnimation();
        }
        
        // int DroppedMainIndex =  (int)info.e_SlimeMainType * SlimeTypesCount + info.SlimeMainIndex;
        // int DroppedMergedIndex =  (int)info.e_SlimeMergedType * SlimeTypesCount + info.SlimeMergedIndex;
        // if (m_DiscoveredSlimes.Count<=0)
        // {
        //     m_DiscoveredSlimes.Add(info);
        //     m_CollectableManager.GetWallet(eCollectable.Coin).Add(m_NewSlimeReward);
        //     m_CollectableManager.GetWallet(eCollectable.UniqueSlimes).Add(1);
        //     return;
        // }
        // foreach (var slimeInfo in m_DiscoveredSlimes)
        // {
        //     int TakenMainIndex =  (int)slimeInfo.e_SlimeMainType * SlimeTypesCount + slimeInfo.SlimeMainIndex;
        //     int TakenMergedIndex =  (int)slimeInfo.e_SlimeMergedType * SlimeTypesCount + slimeInfo.SlimeMergedIndex;
        //     
        //     switch(info.isMerged)
        //     {
        //         case true:
        //             if (DroppedMainIndex == TakenMainIndex && DroppedMergedIndex == TakenMergedIndex && info.isMerged == slimeInfo.isMerged)
        //                 return;
        //             break;
        //         case false:
        //             if (DroppedMainIndex == TakenMainIndex)
        //                 return;
        //             break;
        //     }
        // }
        // m_DiscoveredSlimes.Add(info);
        // m_CollectableManager.GetWallet(eCollectable.Coin).Add(m_NewSlimeReward);
        // m_CollectableManager.GetWallet(eCollectable.UniqueSlimes).Add(1);
    }
    
        #region Save
    private void Load()
    {
        SaveManager.Instance.Load<SaveDiscoveredSlimes>(path,LoadComplete,false);
    }
    private void Save()
    {
        m_SaveSaveDiscoveredSlimes = new SaveDiscoveredSlimes
        {
            m_DiscoveredSlimesToSave = m_DiscoveredSlimes
        };

        SaveManager.Instance.Save(m_SaveSaveDiscoveredSlimes, path, SaveComplete,false);
    }
    public void OnApplicationQuit()
    {
        Save();
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
            Save();
    }

    private void SaveComplete(SaveResult result, string message)
    {
        if (result == SaveResult.Error)
            Debug.LogWarning(message);
        if (result == SaveResult.Success)
            Debug.LogWarning(message);

        //Debug.LogError("Save error и + message");
    }
    private void LoadComplete(SaveDiscoveredSlimes saveSlimesGrid,SaveResult result, string message)
    {
        if (result == SaveResult.Error)
            Debug.LogWarning(message);
        // Debug.LogError("Load error: " + message);

        if (result == SaveResult.Success)
        {
            m_SaveSaveDiscoveredSlimes = saveSlimesGrid;
            m_DiscoveredSlimes = m_SaveSaveDiscoveredSlimes.m_DiscoveredSlimesToSave;
            Debug.LogWarning(message);
            //  Debug.LogError("Success");
        }
    }
    //if по error save was successful
    #endregion

    private CollectableManager m_CollectableManager => CollectableManager.Instance;
}
