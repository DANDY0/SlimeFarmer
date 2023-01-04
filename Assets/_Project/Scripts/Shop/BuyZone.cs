using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BuyZone : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    [SerializeField] private ParticleSystem m_MoneyEffect;
    [SerializeField] private ShopManager m_ShopManager;
    [SerializeField] private Collider m_Collider;
    public int m_DefaultPrice;
    [Title("")]
    [SerializeField, ReadOnly] protected string m_Id;
    [Title("")]
    public int IntID;
    [SerializeField] private GameObject m_BuyingBlock;
    [SerializeField] private TextMeshPro m_PriceDisplay;
    [SerializeField] private GameObject m_BuyingEffect;
    [SerializeField] private BuyType e_BuyType;
    
    protected Coroutine m_BuyingCoroutine;
    protected WaitForSeconds m_StartDelay = new WaitForSeconds(0.5f);
    protected WaitForSeconds m_RemovigDelay = new WaitForSeconds(0.1f);
    
    private const string c_PortalsBought = "PortalsBought";

    protected int m_CurrentPrice;
    protected int CurrentPrice
    {
        get { return m_CurrentPrice; }
        set
        {
            m_CurrentPrice = value;
            m_PriceDisplay.text = m_CurrentPrice.ToString();
        }
    }

    #region Editor
    
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_Id = $"{gameObject.name}";
        m_MoneyEffect = GetComponentInChildren<ParticleSystem>();
        m_BuyingBlock = transform.FindDeepChild<GameObject>("Visual");
        m_PriceDisplay = transform.FindDeepChild<TextMeshPro>("PriceDisplay");
        m_BuyingEffect = transform.FindDeepChild<GameObject>("BuyingEffect");
        m_Collider = GetComponent<Collider>();
        m_ShopManager = FindObjectOfType<ShopManager>();
    }
    #endregion
    
     #region Init
    private void Awake()
    {
      // CurrentPrice = m_DefaultPrice;
      if(e_BuyType == BuyType.Portal)
          m_DefaultPrice = GameplayVariables.PortalDefaultPrices[IntID];
      if (e_BuyType == BuyType.Field)
      {
          m_DefaultPrice = GameplayVariables.FieldDefaultPrices[IntID];
//          FieldTrigger.OnFieldClose.Invoke(IntID);
      }

      if(e_BuyType == BuyType.Portal)
          CurrentPrice = m_StorageManager.GetPortalPrice(m_Id, m_DefaultPrice);
      
      if(e_BuyType == BuyType.Field)
          CurrentPrice = m_StorageManager.GetFieldPrice(m_Id, m_DefaultPrice);
    }

    private void OnEnable()
    {
        GameManager.onLevelLoaded += onLevelLoaded;
    }

    private void OnDisable()
    {
        GameManager.onLevelLoaded -= onLevelLoaded;
    }

    #endregion
    
    #region Callbacks
    private void onLevelLoaded(int i)
    {
        if (StorageManager.Instance.GetPortalPrice(m_Id, m_DefaultPrice) <= 0)
        {
            m_BuyingBlock.SetActive(false);
            //m_Collider.enabled = false;
            Destroy(m_Collider);
        }
    }

    #endregion
    
    #region Specific
    
    #endregion
    
    #region Physics
 #region Physics
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character _))
        {
            stopBuying();
            m_BuyingCoroutine = StartCoroutine(buyCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        stopBuying();
        m_StorageManager.SavePortalPrice(m_Id, CurrentPrice);
    }
    #endregion
    
       #region Buying
    private IEnumerator buyCoroutine()
    {
        yield return m_StartDelay;
        
        m_MoneyEffect.Play();
        int removingStep = (int)(m_DefaultPrice * 0.05f);

        while (true)
        {
            if (m_CoinsWallet.Amount == 0)
            {
                m_BuyingCoroutine = null;
                m_MoneyEffect.Stop();
                yield break;
            }

            removingStep = Mathf.Min(removingStep, m_CurrentPrice);
            removingStep = Mathf.Min(removingStep, m_CoinsWallet.Amount);

            if (m_CoinsWallet.CanRemove(removingStep))
                CurrentPrice -= removingStep;

            if (CurrentPrice <= 0)
            {
                open();
                m_BuyingCoroutine = null;
                m_MoneyEffect.Stop();
                yield break;
            }
            
            yield return m_RemovigDelay;
        }
    }

    protected virtual void open()
    {
        //m_StorageManager.SaveTablePrice(m_Id, CurrentPrice);
        /// save ///
    
        if(m_BuyingEffect!=null)
            m_BuyingEffect.SetActive(true);
        m_BuyingBlock.SetActive(false);
        m_StorageManager.SavePortalPrice(m_Id, CurrentPrice);
        if (e_BuyType == BuyType.Portal)
        {
            m_ShopManager.AllDoors[IntID].CanEntry = true;
            m_ShopManager.portalsBought += 1;
            PlayerPrefs.SetInt(c_PortalsBought , m_ShopManager.portalsBought);
            m_CollectableManager.GetWallet(eCollectable.UniqueSlimes).Add(0);
        }
        if (e_BuyType == BuyType.Field)
        {
            m_ShopManager.FieldsToBuy[IntID].IsOpened = true;
            m_ShopManager.FieldsToBuy[IntID].SetMaterial();
            m_ShopManager.FieldsToBuy[IntID].ChangeCamera();
        }
            

        HapticManager.Instance.Haptic(MoreMountains.NiceVibrations.HapticTypes.Success);
    }
    
    private void stopBuying()
    {
        if (m_BuyingCoroutine != null)
        {
            m_MoneyEffect.Stop();
            StopCoroutine(m_BuyingCoroutine);
            m_BuyingCoroutine = null;
        }
    }

    #endregion
    #endregion
    public enum BuyType
    {
        Portal = 0,
        Field = 1,
    }
    protected StorageManager m_StorageManager => StorageManager.Instance;
    protected CollectableWallet m_CoinsWallet => CollectableManager.Instance.GetWallet(eCollectable.Coin);
    private CollectableManager m_CollectableManager => CollectableManager.Instance;

}
