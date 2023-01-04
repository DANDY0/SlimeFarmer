using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Extensions;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

public class SlimeBlobAnimation : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private ParticleSystem[] m_SmashParticles;
    [SerializeField] private Blob m_SlimeBlob;
    public Material m_TransparentMaterial;
    public Color m_BlobColor;

    public float m_TimeToDestroy;

    private const string c_Color1 = "Color_984f38cfa15340baa777476ede88ef22";
    private const string c_Color2 = "Color_c3a4016b5cd24d5e9964807f243a0d05";

    public bool isCatched;
    
    [Button]
    private void SetRefs()
    {
        m_Animator = GetComponent<Animator>();
    }
    public void DropBlob()
    {
        if (isCatched)
            return;
        var newBlob = Instantiate(m_SlimeBlob, transform.position+new Vector3(0,.01f,0),Quaternion.identity);
        newBlob.transform.eulerAngles = new Vector3(-90,Random.Range(0,360),0);
        var blobRenderer = newBlob.meshRenderer;

        //newBlob.meshRenderer.material.color = m_BlobColor;
        
        var tmp = new Material(newBlob.meshRenderer.material);
        tmp.SetColor(c_Color1,m_BlobColor);
        tmp.SetColor(c_Color2,m_BlobColor);
        blobRenderer.material = tmp;
        
       // ParticleSystem.MainModule settings = m_SmashParticles[0].main;
       // settings.startColor = new ParticleSystem.MinMaxGradient(m_BlobColor);
       
         foreach (var particle in m_SmashParticles)
         {
             var main = particle.main;
             main.startColor = m_BlobColor;
             
         }
         m_SmashParticles[0].Play(true);
    

        newBlob.transform.DOScale(Vector3.zero, m_TimeToDestroy);
        newBlob.transform.DOMoveY(-0.02f, m_TimeToDestroy)
            .OnComplete(()=>Destroy(newBlob));
        //    StartCoroutine(ColorFade(blobRenderer,blobRenderer.material,m_TransparentMaterial, newBlob.gameObject));

    }

    private IEnumerator ColorFade(MeshRenderer renderer,Material material1, Material material2, GameObject toDelete)
    {
        float duration = m_TimeToDestroy;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            renderer.material.Lerp(material1, material2, elapsedTime/duration);
            // m_MeshRenderer.material.Lerp(material1, material2, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(toDelete);
    }
    
}