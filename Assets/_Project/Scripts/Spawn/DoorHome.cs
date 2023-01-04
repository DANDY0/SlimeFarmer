using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class DoorHome : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_Sparks;

    public int DoorBiomID;
    
    public static Action<int> OnGoHome;
    
    
    [Button]
    private void setRefs()
    {
        m_Sparks = transform.FindDeepChild<ParticleSystem>("SparkRadialExplosionYellow");
    }
    private void OnEnable()
    {
//        Character.OnPlayerTeleportedHome += PlaySparks;
        Character.OnTeleportedBiom += PlaySparks;

    }
    private void OnDisable()
    {
//        Character.OnPlayerTeleportedHome -= PlaySparks;
        Character.OnTeleportedBiom -= PlaySparks;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            OnGoHome.Invoke(DoorBiomID);
            m_Sparks.Play();
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
