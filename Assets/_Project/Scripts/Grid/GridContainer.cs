using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class GridContainer : MonoBehaviour
{
    public List<GridSlot> m_Slots;
    
    public List<SlimeInfo> m_GridSlimes;
    public int GridID;

    private void SpawnSlimes()
    {
    }
    
    #region Editor
    
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_Slots = GetComponentsInChildren<GridSlot>().ToList();
        for (int i = 0; i < m_Slots.Count; i++)
        {
            m_Slots[i].SlotID = i; 
            m_Slots[i].GridID = GridID;
        }
            
    }
    #endregion

    public bool IsHaveSpace()
    {
        foreach (var slot in m_Slots)
            if (!slot.IsSlotFull)
                return true; 
        return false;
    }
    public bool IsFirstTwoSlotsFull()
    {
        return m_Slots[0].IsSlotFull && m_Slots[1].IsSlotFull;
    }

    #region Init
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
   
    }

    private void OnDisable()
    {
    }

    #endregion

}
