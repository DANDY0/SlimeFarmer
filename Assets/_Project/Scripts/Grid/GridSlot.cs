using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridSlot : MonoBehaviour
{
  public Vector3 m_OriginPos;
  public int SlotID;
  public int GridID;
  public DragDropSlime m_CurrentSlime;
  public bool IsSlotFull;

  public static Action OnGridAction;
  private void Start()
  {
    m_OriginPos = transform.localPosition;
    CheckChildObject();
  }
  [Button]
  private void SetRef()
  {
    GridID = GetComponentInParent<GridContainer>().GridID;
  }

  private void OnEnable()
  {
    OnGridAction += CheckChildObject;
  }
  private void OnDisable()
  {
    OnGridAction -= CheckChildObject;
  }


  private void CheckChildObject()
  {
    var child = GetComponentInChildren<DragDropSlime>();
    if (child != null)
    {
      m_CurrentSlime = child;
      m_CurrentSlime.SetIDs(GridID, SlotID);
    }
    else
      m_CurrentSlime = null;
    IsSlotFull = m_CurrentSlime != null;
    
   
  }

 
}
