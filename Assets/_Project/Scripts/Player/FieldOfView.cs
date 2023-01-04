using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using Extensions;
public class FieldOfView : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    
    [SerializeField] private ParticleSystem m_MuzzleParticle;
    [SerializeField] private GameObject m_TornadoEffect;
    [SerializeField] private Material m_TornadoMaterial;

    [SerializeField] private BagPack m_BagPack;
    [SerializeField] private Character m_Character;
    [SerializeField] private MeshCollider m_MeshCollider;
    [SerializeField] private MeshRenderer m_MeshRenderer;
    [SerializeField] private Color m_SearchColor;
    [SerializeField] private Color m_BagFullColor;
    [SerializeField] private Color m_Transparent;
    
    [SerializeField] private Slime m_CurrentTarget;
    
    [SerializeField] private float m_AnimDuration;
    
    public static Action<Slime,bool> OnSlimeEnterField;
    public static Action<Slime,bool> OnSlimeExitField;
    public static Action OnSlimeCatched;

    [SerializeField] private float AnimSpeed;
    [SerializeField] private float FadeInDuration;
    [SerializeField] private float OpacityValue;
    private Coroutine tornadoFadeIn;
    [SerializeField] private float m_TurnSmooth;

    
    // public static event Action<SlimeCatchBar,bool> OnSlimeCatching;
   
    #region Editor
    
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        m_MeshCollider = GetComponent<MeshCollider>();
       // m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_Character = transform.parent.GetComponent<Character>();
        m_BagPack = transform.parent.GetComponentInChildren<BagPack>();
    }
    #endregion
    
    #region Init
    private void Start()
    {
        m_AnimDuration = GameplayVariables.FieldAnimDuration;
        SetTransparentColor();
        m_TornadoMaterial.SetFloat("_Opacity", 0);

    }
    private void OnEnable()
    {
     //   OnSlimeCatched += SetTransparentColor;
        OnSlimeCatched += CheckBagState;
        OnSlimeCatched += offMuzzle;
       // DoorHome.OnGoHome += onLoadingFinished;
        BiomTransitionUI.OnLoadingFinished += onLoadingFinished;
        Character.OnPlayerTeleportedHome += FadeOut;
        OnLevelLoaded();
    }

    private void OnDisable()
    {
        //OnSlimeCatched -= SetTransparentColor;
        OnSlimeCatched -= CheckBagState;
        OnSlimeCatched -= offMuzzle;
        BiomTransitionUI.OnLoadingFinished -= onLoadingFinished;
        Character.OnPlayerTeleportedHome -= FadeOut;
    }

    #endregion
    
    #region Callbacks
    private void OnLevelLoaded()
    {
      SetTransparentColor();
    }

    #endregion
    
    #region Specific
    
    #endregion
    
    #region Animations
    private void onLoadingFinished(int i, BiomTransitionUI.TransitionType type)
    {
        switch(type)
        {
            case BiomTransitionUI.TransitionType.Home:
                SetTransparentColor();
                break;
            case BiomTransitionUI.TransitionType.Biom:
                CheckBagState();
                break;
        }
    }
    
    private void SetTransparentColor()
    {
        m_MeshRenderer.material.DOColor(m_Transparent,m_AnimDuration);
    }
    private void SetSearchColor()
    {
        m_MeshRenderer.material.DOColor(m_SearchColor,m_AnimDuration);
    }
    private void SetBagFullColor()
    {
        m_MeshRenderer.material.DOColor(m_BagFullColor,m_AnimDuration);
    }
    
    private void CheckBagState()
    {
        if (BagPack.e_BagStatus == BagStatus.HaveFreeSpace)
        {
            SetSearchColor();
          //  if(m_CurrentTarget!=null)
            //    m_MuzzleParticle.Play();
        }
        
        if (BagPack.e_BagStatus == BagStatus.BagIsFull)
        {
            SetBagFullColor();
           // m_MuzzleParticle.Stop();
        }
        
        print("check: stop");
        m_CurrentTarget = null;
        m_Character.m_CurrentTarget = null;
    }

    private void FadeIn()
    {
        
    }

    private IEnumerator TornadoFadeIn(bool fadeIn)
    {
        if(tornadoFadeIn!=null)
            StopCoroutine(tornadoFadeIn);
        float duration = FadeInDuration;
        float elapsedTime = 0;
        if (fadeIn)
        {
            m_TornadoEffect.SetActive(true);
            m_MuzzleParticle.Play();
            while (elapsedTime < duration)
            {
                m_TornadoMaterial.SetFloat("_Opacity",elapsedTime/duration * OpacityValue);
                elapsedTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            yield break;
        }
        
        elapsedTime = FadeInDuration/4;
        
        while (elapsedTime > 0)
        {
            m_TornadoMaterial.SetFloat("_Opacity",elapsedTime/duration * OpacityValue);
            elapsedTime -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        m_TornadoEffect.SetActive(false);
        m_MuzzleParticle.Stop();
    }

    #endregion
    #region Physics
    private void OnDrawGizmos()
    {
        
    }
    private void Update()
    {
        if (m_CurrentTarget == null)
        {
            m_TornadoEffect.SetActive(false);
            return;
        }
        Debug.DrawLine(m_TornadoEffect.transform.position, m_CurrentTarget.transform.position + new Vector3(0,0.65f,0) , Color.red);
        var ray = m_CurrentTarget.transform.position + new Vector3(0,0.8f,0) - m_TornadoEffect.transform.position;
      //  var angle = Vector3.Angle(m_CurrentTarget.transform.forward, ray);
//        Debug.Log("Angle: " + angle);
        ray.Normalize();
        Debug.DrawRay(m_TornadoEffect.transform.position,  ray * 1, Color.green);

       // if(Mathf.Abs(angle) > 30)
         //   return;
        
       // m_TornadoEffect.transform.rotation = Quaternion.LookRotation(ray);
        
        // var direction = (m_CurrentTarget.transform.position - transform.position);
        // var rotIssue = (Quaternion.AngleAxis(angleIssue, new Vector3(0,1,0)) * direction).normalized;
        // var rotGoal = Quaternion.LookRotation(rotIssue);
        var rotGoal = Quaternion.LookRotation(ray);
        m_TornadoEffect.transform.rotation = Quaternion.Slerp(transform.rotation,  rotGoal, m_TurnSmooth);
        

        // m_TornadoParticle.transform.LookAt(ray);
        //
        //
        // Vector3 relativePos = m_CurrentTarget.transform.position - transform.position;
        //
        // m_TornadoParticle.transform.rotation = Quaternion.LookRotation(m_CurrentTarget.transform.position 
        //                                                                - m_TornadoParticle.transform.position);
        // m_TornadoParticle.transform.rotation = Quaternion.Inverse(m_TornadoParticle.transform.rotation);;
        //
        // Vector3 targetPostition = new Vector3( m_TornadoParticle.transform.position.x, 
        //     m_CurrentTarget.transform.position.y, 
        //     m_CurrentTarget.transform.position.z ) ;
        // m_TornadoParticle.transform.LookAt( targetPostition );
        //
        // var pos = m_CurrentTarget.transform.position - m_TornadoParticle.transform.position;
        // pos.Normalize();
        //
        // // the second argument, upwards, defaults to Vector3.up
        // Quaternion rotation = Quaternion.LookRotation(relativePos, worldVector);
        // m_TornadoParticle.transform.rotation = rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Slime slime))
            return;
        
        if (m_CurrentTarget != null )
            return;

        if ((int)BagPack.e_BagStatus == 1)
            return;
        print("enter: play");
        CheckBagState();
        m_CurrentTarget = slime;
        m_Character.m_CurrentTarget = slime.gameObject;
        m_Character.needToRotate = true;
        tornadoFadeIn = StartCoroutine(TornadoFadeIn(true));
        m_MuzzleParticle.Play();
       OnSlimeEnterField.Invoke(slime, true);
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Slime slime))
            return;

        if (m_CurrentTarget != null)
        {
            tornadoFadeIn = StartCoroutine(TornadoFadeIn(false));
            return;
        }
        if ((int)BagPack.e_BagStatus == 1)
            return;
        CheckBagState();
        m_CurrentTarget = slime;
        m_Character.m_CurrentTarget = slime.gameObject;
        m_Character.needToRotate = true;
        m_MuzzleParticle.Play();
        tornadoFadeIn = StartCoroutine(TornadoFadeIn(true));
        OnSlimeEnterField.Invoke(slime, true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Slime slime))
        { 
            m_CurrentTarget = null;
            m_Character.needToRotate = false;
            m_Character.m_CurrentTarget = null;
            OnSlimeExitField.Invoke(slime, false);
            m_MuzzleParticle.Stop();
            tornadoFadeIn = StartCoroutine(TornadoFadeIn(false));
        }
    }
    public void FadeOut(int _)
    {
        tornadoFadeIn = StartCoroutine(TornadoFadeIn(false));
    }

    private void offMuzzle()
    {
        m_MuzzleParticle.Stop();
        tornadoFadeIn = StartCoroutine(TornadoFadeIn(false));
        //  m_TornadoEffect.SetActive(false);
    }

    public void DeleteTarget()
    {
        m_CurrentTarget = null;
    }

    #endregion
}
