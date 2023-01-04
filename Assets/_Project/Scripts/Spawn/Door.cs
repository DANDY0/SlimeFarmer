using System;
using System.Collections;
using Extensions;
using GameAnalyticsSDK.Setup;
using Sirenix.OdinInspector;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_Sparks;
    public GameObject DoorBuyZone;
    public int DefaultPrice;
    public int DoorBiomID;
    public bool CanEntry;
    public string Id;
    public static event Action<int> OnDoorEntered;

    [Button]
    private void setRefs()
    {
        Id = $"{DoorBuyZone.name}";
        m_Sparks = transform.FindDeepChild<ParticleSystem>("SparkRadialExplosionYellow");
    }
    private void OnEnable()
    {
        GameManager.onLevelLoaded += onLevelLoaded;
        //Character.OnTeleportedBiom += PlaySparks; 
        Character.OnPlayerTeleportedHome += PlaySparks;

    }
    
    private void OnDisable()
    {
        GameManager.onLevelLoaded -= onLevelLoaded;
//       Character.OnTeleportedBiom -= PlaySparks;
       Character.OnPlayerTeleportedHome -= PlaySparks;
    }
    private void onLevelLoaded(int i)
    {
     
    }



    private void OnTriggerEnter(Collider other)
    {
        if(!CanEntry)
            return;
        if (other.TryGetComponent(out Character character))
        {
            OnDoorEntered.Invoke(DoorBiomID);
            m_Sparks.Play();
            HomaConfig.LevelStarted("Level: " + DoorBiomID);
        }
    }

    private void PlaySparks(int i)
    {
        if (i == DoorBiomID)
        {
          //  StartCoroutine(Delay(.6f));
        }
    }
    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_Sparks.Play();
    }
}
