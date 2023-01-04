using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] private ParticleSystem ParticleSystem;

    private void OnValidate()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        StartCoroutine(ParticleDelay());
    }
    private IEnumerator ParticleDelay()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
