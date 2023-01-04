using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
public class SlimeIncome : MonoBehaviour
{
    [SerializeField] private GameObject m_CoinAnimation;
    [SerializeField] private AudioClip m_CoinSound;
    public MergeSlime MergeSlime;
    
    public float TimeToIncome;
    public float TimePassed;
    
    public int MainAmount;
    public int MergedAmount;

  //  public int[] array;

    [Button]
    private void setRefs()
    { 
         //m_CoinAnimation = GetComponentInChildren<ParticleSystem>();
        // Array.Clear(array, 0, array.Length);
    }
    private void Start()
    {
        MergeSlime = GetComponent<MergeSlime>();
       

    }
    private void OnEnable()
    {
        
    }
    public void TimeIncome(SlimeInfo info)
    {
        StopAllCoroutines();
        TimePassed = info.IncomeTimePassed;
        if (TimePassed > TimeToIncome)
            TimePassed = Random.value;
        StartCoroutine(Timer(info.isMerged));
    }
    
    private IEnumerator Timer(bool isMerged)
    {
        var coinsToAdd = isMerged ? MergedAmount : MainAmount;
        
        while (TimePassed<TimeToIncome)
        {
            TimePassed += 0.1f;
            yield return new WaitForSeconds(0.1f);
            if (TimePassed >= TimeToIncome)
            {
                m_CollectableManager.GetWallet(eCollectable.Coin).Add(coinsToAdd);
                // AudioManager.Instance.PlayCoinSound();
                SoundManager.Instance.PlaySFX(m_CoinSound);
                StartCoroutine(DelayAnim());
                TimePassed = 0;
            }
        }
    }
    public int GetRestTime()
    {
        return (int)TimePassed;
    }

    private IEnumerator DelayAnim()
    {
        m_CoinAnimation.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_CoinAnimation.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        //MergeSlime.GetSlimeIncome(this);
    }

    public void offMoney()
    {
        m_CoinAnimation.SetActive(false);
        StopAllCoroutines();
    }

    private CollectableManager m_CollectableManager => CollectableManager.Instance;

}
