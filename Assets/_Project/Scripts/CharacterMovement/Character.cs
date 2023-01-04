using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
   
   [SerializeField] private BagPack m_BagPack;
   [SerializeField] private FieldOfView m_FieldOfView;
   [SerializeField] private JoystickForMovement m_JoystickMovement;
   [SerializeField] private CharacterMovement m_CharacterMovement;
   [SerializeField] private Transform[] m_DoorTransforms;
   [SerializeField] private Transform m_HomeParent;
   [SerializeField] private Vector3 m_RotateIssue;
   public CharacterAnimations m_CharacterAnimations;
   public Transform WeaponMuzzle;

   public bool needToRotate;
   public GameObject m_CurrentTarget;
   
   public static Action<Transform,Transform> OnPlayerTeleportedBiom;
   public static Action<int> OnPlayerTeleportedHome;
   public static Action<int> OnTeleportedBiom;
   [SerializeField] private float m_TurnSmooth;
   public static int currentFieldID;
   [Header("Creatives")]
   [SerializeField] private Transform LookAtTransform;

   public ParticleSystem m_Cry;  
   
   #region Editor
   private void OnValidate()
   {
      setRefs();
   }
   private void setRefs()
   {
      m_CharacterMovement = GetComponent<CharacterMovement>();
      m_JoystickMovement = GetComponent<JoystickForMovement>();
      m_CharacterAnimations = GetComponent<CharacterAnimations>();
      m_BagPack = GetComponentInChildren<BagPack>();
      m_FieldOfView = GetComponentInChildren<FieldOfView>();
   }
   #endregion

   #region Callbacks
   
   private void Start()
   {
      Application.targetFrameRate = 100;
      HomaConfig.GameplayStarted();
   }
   
   private void OnEnable()
   {
      OnPlayerTeleportedBiom += onBiomLoadFinished;
      OnPlayerTeleportedHome += onTeleportedHome;
   }
   private void OnDisable()
   {
      OnPlayerTeleportedBiom -= onBiomLoadFinished;
      OnPlayerTeleportedHome -= onTeleportedHome;
   }
  #endregion

   private void LateUpdate()
   {
      if (needToRotate && m_CurrentTarget != null)
      {
        // transform.LookAt(m_CurrentTarget.transform);
         var angleIssue = 0;
         if (!InputManager.Instance.IsInputDown)
         {
            angleIssue = 30; 
            InputManager.Instance.resetValues();
           // InputManager.Instance.JoystickDirection = Vector2.zero;
            //  Debug.Log();
         }
         var direction = (m_CurrentTarget.transform.position - transform.position);
         var rotIssue = (Quaternion.AngleAxis(angleIssue, new Vector3(0,1,0)) * direction).normalized;
         var rotGoal = Quaternion.LookRotation(rotIssue);
         transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, m_TurnSmooth);
     
      }
       
      
   }
   private void onBiomLoadFinished(Transform newPosition, Transform newParent)
   {
      m_CharacterMovement.m_CharacterController.enabled = false;
      gameObject.transform.position = newPosition.position;
      gameObject.transform.SetParent(newParent);
      m_CharacterMovement.m_CharacterController.enabled = true;
     
   }
   
   private void onTeleportedHome(int DoorID)
   {
      needToRotate = false;
      m_CharacterMovement.m_CharacterController.enabled = false;
      gameObject.transform.SetParent(m_HomeParent);
      gameObject.transform.position = m_DoorTransforms[DoorID].position;
      m_CharacterMovement.m_CharacterController.enabled = true;
      HomaConfig.LevelCompleted();
   }

   public void TornadoFadeOut()
   {
      m_FieldOfView.FadeOut(0);
   }

   public void Cry()
   {
      m_Cry.Play();
   }
   
   public void CryMoney()
   {
      transform.DOLookAt(LookAtTransform.position,1f)
         .OnComplete(()=>m_Cry.Play());
      
      //m_CharacterAnimations.CryMoney();
   }


}
