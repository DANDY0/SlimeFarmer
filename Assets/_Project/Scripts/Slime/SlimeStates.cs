using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class SlimeStates : MonoBehaviour
{
   [SerializeField] private NavMeshAgent m_MeshAgent;
   [SerializeField] private SkinnedMeshRenderer m_Displacement;
   [SerializeField] private SkinnedMeshRenderer m_DisplacementFace;
   [SerializeField] private float m_RunSpeed;
   [SerializeField] private float m_WalkSpeed;
   [SerializeField] private float m_SlowSpeed;
   [SerializeField] private float minDisplacementValue;
   [SerializeField] private float maxDisplacementValue;
   public Character Character;
   public Animator Animator;
   public float SlimeDistanceRun = 4f;
   public bool isIdle;

   public string c_Idle = "Idle";
   public string c_Jump = "Jump";
   [SerializeField] private Vector2Int m_WonderTime;
   [SerializeField] private float m_CurrentWonderTime;
   [SerializeField] private Vector3 newPos;
   private Coroutine m_MoveWonder;
   private Coroutine m_DisplacementCoroutine;
   
   
   private const string c_Displacement = "_Displacement";
   private void OnValidate()
   {
      m_MeshAgent = GetComponent<NavMeshAgent>();
   }
   private void OnEnable()
   {
      Character = FindObjectOfType<Character>();
      Animator = GetComponentInChildren<Animator>();
      m_Displacement.material.SetFloat(c_Displacement, minDisplacementValue);

     // m_MoveWonder = StartCoroutine(MoveWonderDirection());
    // m_MeshAgent.speed = m_WalkSpeed;
    // m_MoveWonder = StartCoroutine(MoveWonderDirection());
   }

   private void Update()
   {
      float distance = Vector3.Distance( transform.position, Character.transform.position);

      if (distance < SlimeDistanceRun)
      {
         
         Vector3 dirToChar = transform.position - Character.transform.position;
         Vector3 newPos =  transform.position + dirToChar;
         m_MeshAgent.SetDestination(newPos);
         
         if (isIdle)
         {
            if (m_MoveWonder != null)
               StopCoroutine(m_MoveWonder);
            
            //animation
            m_MeshAgent.speed = m_RunSpeed;
            Animator.SetTrigger(c_Jump);
            isIdle = false;
         }
      }
      else
      {
         if (!isIdle)
         {
            Animator.SetTrigger(c_Idle);
            m_MeshAgent.speed = m_WalkSpeed;
            m_MoveWonder = StartCoroutine(MoveWonderDirection());
            //animation
            isIdle = true;
            return;
         }
         if (m_MoveWonder == null)
         {
            m_MeshAgent.speed = m_WalkSpeed;
            m_MoveWonder = StartCoroutine(MoveWonderDirection());
         }
      }
   }

   private IEnumerator MoveWonderDirection()
   {
      m_CurrentWonderTime = Random.Range(m_WonderTime.x,m_WonderTime.y);
      newPos = transform.position + new Vector3(Random.Range(-5,5),0,Random.Range(-5,5));
      m_MeshAgent.SetDestination(newPos);
      yield return new WaitForSeconds(Random.Range(1, 2));
      while (m_CurrentWonderTime > 0)
      {
         // transform.Translate(Vector3.forward * m_MovementSpeed );
       
         m_CurrentWonderTime -= 0.1f;
         yield return new WaitForSeconds(0.1f);
      }
      WonderDirection();
   }
   public void displacementState(bool isInFOV)
   {
      StartCoroutine(DisplacementBiom(isInFOV));
   }
   private IEnumerator DisplacementBiom(bool isInFOV)
   {
      float duration = .5f;
      float elapsedTime = 0;
      
      if (isInFOV)
      {
         while (elapsedTime < duration)
         {
            m_Displacement.material.SetFloat(c_Displacement, elapsedTime/duration * maxDisplacementValue);
            m_DisplacementFace.material.SetFloat(c_Displacement, elapsedTime/duration * maxDisplacementValue + 0.05f);

            // m_MeshRenderer.material.Lerp(material1, material2, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
         }
         yield break;
      }
      elapsedTime = duration;
      while (elapsedTime > minDisplacementValue)
      {
         m_Displacement.material.SetFloat(c_Displacement, elapsedTime/duration * maxDisplacementValue);
         m_DisplacementFace.material.SetFloat(c_Displacement, elapsedTime/duration * maxDisplacementValue+0.05f);
         // m_MeshRenderer.material.Lerp(material1, material2, elapsedTime/duration);
         elapsedTime -= Time.deltaTime;
         yield return null;
      }
      // m_Displacement.material.SetFloat(c_Displacement, minDisplacementValue);
      
   }


   
   
   private void WonderDirection()
   {
      StopCoroutine(m_MoveWonder);
      StartCoroutine(MoveWonderDirection());
   }

   public void SetSlowSpeed()
   {
      m_MeshAgent.speed = m_SlowSpeed;
   }
   public void SetRunSpeed()
   {
      m_MeshAgent.speed = m_RunSpeed;
   }
   
}
