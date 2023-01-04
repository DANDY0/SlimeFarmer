using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
public class ShopManager : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

      public Door[] AllDoors;
      public BuyZone[] DoorBuyZones;
      
      public FieldTrigger[] FieldsToBuy;
      public BuyZone[] FieldBuyZones;
      private const string c_PortalsBought = "PortalsBought";

      public int portalsBought;

    #region Editor
    
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        DoorBuyZones = GetComponentsInChildren<BuyZone>();
        
        for (int i = 0; i < DoorBuyZones.Length; i++)
        {
            DoorBuyZones[i].IntID = i;
        }
        for (int i = 0; i < FieldBuyZones.Length; i++)
        {
            FieldBuyZones[i].IntID = i;
        }

        for (int i = 0; i < AllDoors.Length; i++)
        {
            AllDoors[i].Id = $"{DoorBuyZones[i].gameObject.name}";
            DoorBuyZones[i].m_DefaultPrice = AllDoors[i].DefaultPrice;
        }

        for (int i = 0; i < FieldsToBuy.Length; i++)
        {
            FieldsToBuy[i].Id = $"{FieldBuyZones[i].gameObject.name}";
            FieldBuyZones[i].m_DefaultPrice = FieldsToBuy[i].DefaultPrice;
            FieldsToBuy[i].DefaultPrice = StorageManager.Instance.GetFieldPrice(FieldsToBuy[i].Id, GameplayVariables.FieldDefaultPrices[i]);
        }
    }

    #endregion
    
    #region Init
    private void Awake()
    {
        portalsBought = PlayerPrefs.GetInt(c_PortalsBought);
    }
    private void OnEnable()
    {
        GameManager.onLevelLoaded += OnLevelLoaded;
    }
    private void OnDisable()
    {
        GameManager.onLevelLoaded -= OnLevelLoaded;
    }

    #endregion
    
    #region Callbacks
    private void OnLevelLoaded(int i)
    {
        for (int j = 0; j < AllDoors.Length; j++)
        {
            var bought = StorageManager.Instance.GetPortalPrice(AllDoors[j].Id, AllDoors[j].DefaultPrice) == 0;
            AllDoors[j].CanEntry = bought;
            if (bought)
            {
                DoorBuyZones[j].enabled = false;
            }
            
        }

        for (int k = 0; k < FieldsToBuy.Length; k++)
        {
            var bought = StorageManager.Instance.GetFieldPrice(FieldsToBuy[k].Id, GameplayVariables.FieldDefaultPrices[k]) == 0;
            FieldsToBuy[k].IsOpened = bought;
            FieldsToBuy[k].SetMaterial();
            if (bought)
                FieldBuyZones[k].enabled = false;
        }
    }

    private void onDoorOpened()
    {
        
    }

    #endregion
    
    #region Specific
    
    #endregion
    
    #region Physics
    
    #endregion
    //public 
}
