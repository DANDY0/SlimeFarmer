using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class DragDropSlime : MonoBehaviour
{
    private GameplayVariablesEditor m_GamePlayParameters => GameConfig.Instance.Gameplay;
    
    [SerializeField] private MergeSlime m_MergeSlime;
    [SerializeField] private GridSlot m_CurrentSlot;
    [SerializeField, ReadOnly] private Collider m_Collider;
    [SerializeField, ReadOnly] private Vector3 m_CameraDragOffset;
    
    private const string IsSlimeDeleted = "IsSlimeDeleted";
    private const string IsSlimeAdded = "IsSlimeAdded";

    public static Action<bool> OnDrag;
    public static Action<DragDropSlime> OnDeleteSlime;

    [ReadOnly] public bool m_CanDrag;
    private float m_OffsetZ;
    private float m_CardMaxUpValue => m_GamePlayParameters.CardMaxUpValue;
    private float m_MaxDistance => m_GamePlayParameters.MaxRaycastDistance;
    public float m_SmoothValue => m_GamePlayParameters.SmoothAnimValue;
    public float m_AnimNewSlotSpeed => m_GamePlayParameters.AnimNewSlotSpeed;
    public float m_AnimSpeedParent => m_GamePlayParameters.AnimSpeedParent;
    
    public Vector3 m_OriginPos;
    public Vector3 m_BeforeDragPos;
    private Vector3 velocity = Vector3.zero;
    public bool isThisField;
    [SerializeField] private Camera mainCamera;
    
    #region Editor
    
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_Collider = GetComponent<Collider>();
        m_MergeSlime = GetComponent<MergeSlime>();
        mainCamera = Camera.main;
    }
    #endregion
    
    #region Init

    private void Awake()
    {
        m_OriginPos = transform.localPosition;
    }

    private void OnEnable()
    {
        FieldTrigger.OnCanDrag += onCanDrag;
        FieldTrigger.OnEnterDropZone += IsThisField;
        //  OnDeleteSlime += onDeleteSlime;
    }
    private void OnDisable()
    {
        FieldTrigger.OnCanDrag -= onCanDrag;
        FieldTrigger.OnEnterDropZone -= IsThisField;
        // OnDeleteSlime -= onDeleteSlime;
    }
    void onCanDrag(bool state) => m_CanDrag = state;
   
    
    #endregion
    
    #region Drag
      private Vector3 MouseWorldPosition()
      {
          var mouseScreenPos = Input.mousePosition;
          mouseScreenPos.z = mainCamera.WorldToScreenPoint(transform.position).z;
          return mainCamera.ScreenToWorldPoint(mouseScreenPos);
          
      }
      private void OnMouseDown()
      {
          if(!m_CanDrag)
              return;
          if(!isThisField)
              return;
          //Pointer.onShowPointer.Invoke(true);
          m_BeforeDragPos = gameObject.transform.localPosition;
          m_CameraDragOffset = transform.position- MouseWorldPosition();
         // m_Collider.enabled = false;
          OnDrag.Invoke(true);
      }
      private void OnMouseDrag()
      {
          if(!m_CanDrag)
              return;
          if(!isThisField)
              return;
          
          var upPos = transform.position;
          upPos = MouseWorldPosition() + m_CameraDragOffset ;
         // var hitsInfo = RaycastHitsInfo();

          transform.position = Vector3.SmoothDamp(upPos,
              new Vector3(upPos.x, m_CardMaxUpValue, upPos.z),ref velocity,m_SmoothValue);
      }
      private void OnMouseUp()
      {
          if(!m_CanDrag)
              return;
          if(!isThisField)
              return;
          // Pointer.onShowPointer.Invoke(false);
          var hitsInfo = RaycastHitsInfo();
          
          if (findNewSlot(hitsInfo))
              return;
          if (isDeleteZone(hitsInfo))
              return;
          if (isAddZone(hitsInfo))
              return;
          
          returnSlime();
          m_Collider.enabled = true;
          OnDrag.Invoke(false);

      }
      #endregion
    
    #region SlimeActions
    private bool findNewSlot(RaycastHit[] i_HitsInfo)
    {
        foreach (var hit in i_HitsInfo)
            if (hit.transform.TryGetComponent(out GridSlot slot) /* && m_ThisCard.m_CurrentSlot.m_SlotID != slot.m_SlotID*/)
            {
                if (slot.m_CurrentSlime !=null && slot.m_CurrentSlime == this)
                    return false;
                
                if (slot.GridID != m_MergeSlime.GetSlimeInfo().GridID)
                {
                   return false;
                }
                    
                
                bool canMerge = slot.m_CurrentSlime != null;
              
                switch(canMerge)
                {
                    case true:
                        if (m_MergeSlime.m_SlimeInfo.isMerged)
                            return false;
                        if (slot.m_CurrentSlime.m_MergeSlime.m_SlimeInfo.isMerged)
                            return false;
                        mergeSlimes(m_MergeSlime,slot.m_CurrentSlime.m_MergeSlime);
                        return true;
                        break;
                    case false:
                        placeSlime(slot);
                        return true;
                        break;
                }
            }
        return false;
    }
    private bool isDeleteZone(RaycastHit[] i_HitsInfo)
    {
        foreach (var hit in i_HitsInfo)
            if (hit.transform.TryGetComponent(out DeleteZone slot) /* && m_ThisCard.m_CurrentSlot.m_SlotID != slot.m_SlotID*/) {
                onDeleteSlime();
                OnDrag.Invoke(false);
                if (PlayerPrefs.GetInt(IsSlimeDeleted) == 0)
                {
                    TutorialManager.Instance.OnDeleteTutorDisabled.Invoke();
                    PlayerPrefs.SetInt(IsSlimeDeleted,1);
                }
                return true;
            }
        return false;
    }
    private bool isAddZone(RaycastHit[] i_HitsInfo)
    {
        foreach (var hit in i_HitsInfo)
            if (hit.transform.TryGetComponent(out AddZone slot) /* && m_ThisCard.m_CurrentSlot.m_SlotID != slot.m_SlotID*/) {
                onAddBagSlime();
                OnDrag.Invoke(false);
                if (PlayerPrefs.GetInt(IsSlimeAdded) == 0)
                {
                    TutorialManager.Instance.OnAddTutorDisabled.Invoke();
                    PlayerPrefs.SetInt(IsSlimeAdded,1);
                }

                return true; 
            }
        return false;
    }
    private void placeSlime(GridSlot newSlot)
    {
        transform.SetParent(newSlot.transform);

        Vector3 newPos = new Vector3(0,0,.4f);
  
        StartCoroutine(MoveToTarget(gameObject, newPos, m_AnimNewSlotSpeed));
        
        m_Collider.enabled = true;
        OnDrag.Invoke(false);
        GridSlot.OnGridAction.Invoke();
    }
    private void mergeSlimes(MergeSlime slimeDropped, MergeSlime slimeToMerge)
    {
        SlimeMergeManager.Instance.MergeSlimes(slimeDropped, slimeToMerge);
        m_Collider.enabled = true;
        OnDrag.Invoke(false);
    }
    private void returnSlime()
    {
        StartCoroutine(MoveToTarget(gameObject, m_BeforeDragPos, m_AnimSpeedParent));
        m_Collider.enabled = true;  
        OnDrag.Invoke(false);
        GridSlot.OnGridAction.Invoke();
    }
  #endregion

    #region Special
    private RaycastHit[] RaycastHitsInfo()
    {
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - rayOrigin;
          
        RaycastHit[] hitsInfo;
        hitsInfo = Physics.RaycastAll(rayOrigin, rayDirection, m_MaxDistance);
        return hitsInfo;
    }
  
    private IEnumerator MoveToTarget(GameObject i_Card,Vector3 i_Target, float i_Speed)
    {
        while (Vector3.Distance(i_Card.transform.localPosition,i_Target)>i_Speed*Time.deltaTime){
            i_Card.transform.localPosition = 
                Vector3.MoveTowards(i_Card.transform.localPosition, i_Target, i_Speed*Time.deltaTime);
            yield return 0;
        }
        i_Card.transform.localPosition = i_Target;
    }
    private void onDeleteSlime()
    {
        deleteAnimation();
    }
    private void onAddBagSlime()
    {
        if (BagPack.e_BagStatus == BagStatus.BagIsFull)
        {
            returnSlime();
            return;
        }
        BagPack.OnSlimeAdded.Invoke(m_MergeSlime.GetSlimeInfo());
        addAnimation();
    }

  #endregion
 
    #region Animation
    private void deleteAnimation()
    {
        transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack)
            .OnComplete(DestroySlime);
    }
    private void addAnimation()
    {
        transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack)
            .OnComplete(DestroySlime);
    }

    private void DestroySlime()
    {
        Destroy(gameObject);
    }
    #endregion
    private void OnDestroy()
    {
        GridSlot.OnGridAction?.Invoke();
    }
    
    public SlimeInfo GetSlimeInfo()
    {
        return m_MergeSlime.m_SlimeInfo;
    }

    public void SetIDs(int gridID, int slotID)
    {
        m_MergeSlime.SetIDs(gridID,slotID);
    }
    public void SetPassedTime(float timePassed)
    {
        m_MergeSlime.m_SlimeInfo.IncomeTimePassed = timePassed;
    }
    private void IsThisField(bool state, int gridID)
    {
        if (state)
        {
            if (gridID == GetGridID())
            {
                isThisField = true;
                return;
            }
            isThisField = false;
            return;
        }
        isThisField = false;

    }
    public int GetGridID()
    {
        return m_MergeSlime.m_SlimeInfo.GridID;
    }
}
