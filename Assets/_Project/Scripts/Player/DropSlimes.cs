using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameAnalyticsSDK.Setup;
using Sirenix.OdinInspector;
using UnityEngine;

public class DropSlimes : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    [SerializeField] private BagPack m_BagPack;
    [SerializeField] private AudioClip m_SlimeLand;
    [SerializeField] private MergeSlime[] m_SlimeTypes;
    [SerializeField] private GridContainer[] m_GridContainer;
    [SerializeField] private bool m_CanDrop;
    [SerializeField] private Transform m_BagPos;
    [SerializeField] private Transform RingSlimeParent;
    private Vector3 GridSlimeUpUpOffset => GameplayVariables.GridSlimeUpOffset;
    private Vector3 m_FirstScaleValue => GameplayVariables.FirstScaleValue;
    private Vector3 m_SecondScaleValue => GameplayVariables.SecondScaleValue;
    private Vector3 m_SlimeRotateError => GameplayVariables.SlimeRotateError;
    private Vector3 m_SlimeRotateErrorRing => GameplayVariables.SlimeRotateErrorRing;
    private float TimeBetweenSlimeDrop => GameplayVariables.TimeBetweenSlimeDrop;
    private float DropSlimeDelay =>GameplayVariables.DropSlimeDelay;

    #region Editor
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_BagPack = GetComponent<BagPack>();
       // m_GridContainer = FindObjectOfType<GridContainer>();
    }
    #endregion
    
     #region Init
    private void Start()
    {
       
    }

    private void OnEnable()
    {
        FieldTrigger.OnEnterDropZone += onDropZoneEntered;
        DropInRing.OnRingEntered += onRingEntered;
        GameManager.onLevelLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        FieldTrigger.OnEnterDropZone -= onDropZoneEntered;
        GameManager.onLevelLoaded -= OnLevelLoaded;
        DropInRing.OnRingEntered -= onRingEntered;
    }

    #endregion
    #region Creatives
    private void onRingEntered()
    {
        StartCoroutine(dropSlimeRing());
    }
    private IEnumerator dropSlimeRing()
    {
        yield return new WaitForSeconds(DropSlimeDelay);
        while (m_BagPack.CountInBag > 0)
        {
            var slimeToDrop = m_BagPack.m_SlimesInBag[m_BagPack.m_SlimesInBag.Count-1];
            BagPack.OnSlimeDropped.Invoke(slimeToDrop);
            InstantiateSlimeRing(slimeToDrop);
            yield return new WaitForSeconds(TimeBetweenSlimeDrop);
        }
        yield return null;
    }
    private void InstantiateSlimeRing(SlimeInfo slimeInfo)
    {
        Transform slimeParent = null;
        
        slimeParent = RingSlimeParent;
        var mergeSlime = Instantiate(m_SlimeTypes[(int)slimeInfo.e_SlimeMainType],m_BagPos.position,Quaternion.identity,slimeParent);
        
        if(!slimeInfo.isMerged)
            mergeSlime.InitSlime(slimeInfo);
        else
            mergeSlime.MergeSlimeDrop(slimeInfo);
        
        GridSlot.OnGridAction.Invoke();
      //  mergeSlime.transform.eulerAngles = m_SlimeRotateErrorRing;
        DropAnimationRing(slimeInfo.isMerged,mergeSlime,slimeParent);
    }
    private void DropAnimationRing(bool isMerged,MergeSlime mergeSlime, Transform endPosition)
    {
        var duration = 1f;
        
        mergeSlime.transform.localScale = new Vector3(0,0,0);
        
        if(!isMerged)
            mergeSlime.transform.DOScale(m_FirstScaleValue, duration);
        else
            mergeSlime.transform.DOScale(m_SecondScaleValue, duration);
        
        StartCoroutine(onAnimEnd(duration));
        mergeSlime.transform.DOLocalRotate(new Vector3(0,90,0),1f, RotateMode.LocalAxisAdd);
        mergeSlime.transform.DOJump(endPosition.position,2,1,duration)
            .OnComplete(AnimRingCallback);
        
    }
    private void AnimRingCallback()
    {
        
        //FieldTrigger.OnCanDrag?.Invoke(true)
    }

    #endregion
    #region Callbacks
    private void OnLevelLoaded(int i)
    {
        m_GridContainer = GridsManager.Instance.Grids;
        LoadSlimes();
    }

    #endregion
    #region Specific

    private void onDropZoneEntered(bool state,int id)
    {
        if(state)
            StartCoroutine(dropSlimes(id));
        else
            StopAllCoroutines();
    }
    private IEnumerator dropSlimes(int id)
    {
        yield return new WaitForSeconds(DropSlimeDelay);
        while (m_BagPack.CountInBag > 0 && m_GridContainer[id].IsHaveSpace())
        {
            var slimeToDrop = m_BagPack.m_SlimesInBag[m_BagPack.m_SlimesInBag.Count-1];
            BagPack.OnSlimeDropped.Invoke(slimeToDrop);
            InstantiateSlime(slimeToDrop,id);
            yield return new WaitForSeconds(TimeBetweenSlimeDrop);
        }
        yield return null;
    }
    private void InstantiateSlime(SlimeInfo slimeInfo, int id)
    {
        Transform slimeParent = null;
        foreach (var slot in m_GridContainer[id].m_Slots)
        {
            if (!slot.IsSlotFull)
            {
                slimeParent = slot.transform;
                slimeInfo.SlotID = slot.SlotID;
                slimeInfo.GridID = slot.GridID;
                break;
            }
        }
        var mergeSlime = Instantiate(m_SlimeTypes[(int)slimeInfo.e_SlimeMainType],m_BagPos.position,Quaternion.identity,slimeParent);

        if(!slimeInfo.isMerged)
            mergeSlime.InitSlime(slimeInfo);
        else
            mergeSlime.MergeSlimeDrop(slimeInfo);
        
        GridSlot.OnGridAction.Invoke();
        mergeSlime.transform.eulerAngles = m_SlimeRotateError;
        DropAnimation(slimeInfo.isMerged,mergeSlime,slimeParent);
    }
    private void DropAnimation(bool isMerged,MergeSlime mergeSlime, Transform endPosition)
    {
        var duration = 1f;
        
        mergeSlime.transform.localScale = new Vector3(0,0,0);
        if(!isMerged)
            mergeSlime.transform.DOScale(m_FirstScaleValue, duration);
        else
            mergeSlime.transform.DOScale(m_SecondScaleValue, duration);
        StartCoroutine(onAnimEnd(duration));
     //   mergeSlime.transform.DOLocalRotate(new Vector3(0,-90,0),1f, RotateMode.LocalAxisAdd);
        mergeSlime.transform.DOJump(endPosition.position + GridSlimeUpUpOffset,2,1,duration)
            .OnComplete(() =>FieldTrigger.OnCanDrag?.Invoke(true));
            
    }

    private IEnumerator onAnimEnd(float duration)
    {
        duration *= 0.65f;
        yield return new WaitForSeconds(duration);
        SoundManager.Instance.PlaySFX(m_SlimeLand);
    }
    
    #endregion

    
    
    #region Save
    private void LoadSlimes()
    {
        //  var slimeInfo = SlimeMergeManager.Instance.m_SaveSlimesGrid.SlimeInfoToSave[0];
        var slimesToLoad = SlimeMergeManager.Instance.m_SaveSlimesGrid?.SlimeInfoToSave;
        if(slimesToLoad==null)
            return;
        foreach (var slimeInfo in slimesToLoad)
        {
            Transform slimeParent = null;
    
            slimeParent = m_GridContainer[slimeInfo.GridID].m_Slots[slimeInfo.SlotID].transform;
            var mergeSlime = Instantiate(m_SlimeTypes[(int)slimeInfo.e_SlimeMainType],slimeParent.position+GridSlimeUpUpOffset,Quaternion.identity,slimeParent);
        
            if(!slimeInfo.isMerged)
                mergeSlime.InitSlime(slimeInfo);
            else
                mergeSlime.MergeSlimeDrop(slimeInfo);
        
            GridSlot.OnGridAction.Invoke();
            mergeSlime.transform.eulerAngles = m_SlimeRotateError;
            
            mergeSlime.transform.localScale = slimeInfo.isMerged ? m_FirstScaleValue : m_SecondScaleValue;
            //mergeSlime.transform.DOScale(m_FirstScaleValue, .1f);
            mergeSlime.transform.DOScale(!slimeInfo.isMerged ? m_FirstScaleValue : m_SecondScaleValue, .2f);

        }

        // DropAnimation(slimeInfo.isMerged,mergeSlime,slimeParent);
    }
  #endregion
   
    }

