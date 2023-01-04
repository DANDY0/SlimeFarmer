using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class StorageManager : StorageManagerBase
{
    public static event Action onCoinsAmountChanged;
    
    [Title("Currency")]
    [ShowInInspector, PropertyOrder(0)]
    public int CoinsAmount
    {
        get
        {
            return GetCollectable(eCollectable.Coin);
        }
        set
        {
            onCoinsAmountChanged?.Invoke();
            SetCollectable(eCollectable.Coin, value);
        }
    }
    
  
    public void SavePortalPrice(string id, int price)
    {
        PlayerPrefs.SetInt(id, price);
    }
    public int GetPortalPrice(string id, int defaultPrice) => PlayerPrefs.GetInt(id, defaultPrice);
    
    public void SaveFieldPrice(string id, int price)
    {
        PlayerPrefs.SetInt(id, price);
    }
    public int GetFieldPrice(string id, int defaultPrice) => PlayerPrefs.GetInt(id, defaultPrice);



}
