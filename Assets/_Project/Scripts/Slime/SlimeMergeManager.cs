using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SlimeMergeManager : Singleton<SlimeMergeManager>
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;
    [SerializeField] private AudioClip m_UnlockSound;
    public GridContainer[] GridContainers;
    public SaveSlimesGrid m_SaveSlimesGrid;
    [SerializeField] private Particle m_Particles;
    
    const string fileName = "SaveGridSlimes";
    private const string IsSlimeMerged = "IsSlimeMerged";

    string path;
    
    private void Awake()
    {
        path = Application.persistentDataPath + "/" + fileName;
        
        print(path);
        
        Load();
    }
    public void MergeSlimes(MergeSlime slimeDropped, MergeSlime slimeToMerge)
    {
        var pos = slimeToMerge.transform.position + new Vector3(0,.3f,0);
        Instantiate(m_Particles,pos,Quaternion.identity);
        int droppedIndex = slimeDropped.m_SlimeInfo.SlimeMainIndex;
        slimeToMerge.MergeSlimeInit(droppedIndex, slimeDropped.m_SlimeColors[droppedIndex], slimeDropped.m_SlimeInfo.e_SlimeMainType);
        DisappearAnimation(slimeDropped,slimeToMerge);
        
        GridSlot.OnGridAction.Invoke();
        SoundManager.Instance.PlaySFX(m_UnlockSound);
      
        if (PlayerPrefs.GetInt(IsSlimeMerged) != 0)
            return;
        TutorialManager.Instance.OnMergeTutorDisabled.Invoke();
        PlayerPrefs.SetInt(IsSlimeMerged,1);
    }
    private void DisappearAnimation(MergeSlime slimeDropped,MergeSlime slimeToMerge)
    {
        slimeToMerge.transform.DOScale(.8f * Vector3.one, .3f)
            .OnComplete(()=>SlimeMergedEffectMethod(slimeToMerge));
        slimeDropped.transform.DOMove(slimeToMerge.transform.position, 1f);
        slimeDropped.transform.DOScale(Vector3.zero, 1f)
            .OnComplete(() => Destroy(slimeDropped.gameObject));
    }

    private void SlimeMergedEffectMethod(MergeSlime slimeToMerge)
    {
        slimeToMerge.transform.DOScale(GameplayVariables.SecondScaleValue, .5f).SetEase(Ease.InBounce);
    }


    #region Save
    private void Load()
    {
        SaveManager.Instance.Load<SaveSlimesGrid>(path,LoadComplete,false);
    }
    private void Save()
    {
        var listToSave = new List<SlimeInfo>();
   
        foreach (var container in GridContainers)
        {
            foreach (var gridSlot in container.m_Slots)
            {
                if (gridSlot.IsSlotFull)
                {
                    gridSlot.m_CurrentSlime.SetPassedTime(gridSlot.m_CurrentSlime.GetComponent<SlimeIncome>().TimePassed);
                    listToSave.Add(gridSlot.m_CurrentSlime.GetSlimeInfo());
                }
            }
        }
        m_SaveSlimesGrid = new SaveSlimesGrid
        {
            SlimeInfoToSave = listToSave
        };

        SaveManager.Instance.Save(m_SaveSlimesGrid, path, SaveComplete,false);
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

        //Debug.LogError("Save error и + message");
    }
    private void LoadComplete(SaveSlimesGrid saveSlimesGrid,SaveResult result, string message)
    {
        if (result == SaveResult.Error)
            Debug.LogWarning(message);
        // Debug.LogError("Load error: " + message);

        if (result == SaveResult.Success)
        {
            m_SaveSlimesGrid = saveSlimesGrid;
            Debug.LogWarning(message);
            //  Debug.LogError("Success");
        }
        
           
    }
    //if по error save was successful
    #endregion
   


}