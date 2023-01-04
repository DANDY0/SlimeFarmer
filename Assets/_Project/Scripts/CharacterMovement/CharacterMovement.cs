using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMovement : MonoBehaviour
{
    private InputVariablesEditor InputVariables => GameConfig.Instance.Input;
    [SerializeField] private Character m_Character;
    public CharacterController m_CharacterController;
    
    [Header("Character movement stats")]
   
    private float m_GravityForce;
    private Vector3 m_VelocityDirection;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_MoveSpeed;
    public event Action OnPlayedDead;

    #region Init
    private void Start()
    {
        m_Character = GetComponent<Character>();
    }
    private void OnEnable()
    {
        OnPlayedDead += BlockMovement;
        GameManager.onLevelStarted += onLevelStarted;
        DragDropSlime.OnDrag += ChangeMovementState;
    }
    private void onLevelStarted()
    {
        m_GravityForce = InputVariables.GravityForce;
        m_RotationSpeed = InputVariables.RotationSpeed;
        m_MoveSpeed = InputVariables.MoveSpeed;
        m_CharacterController.transform.localPosition = new Vector3(0,50,0);
    }

    private void OnDisable()
    {
        OnPlayedDead -= BlockMovement;
        GameManager.onLevelStarted -= onLevelStarted;
        DragDropSlime.OnDrag -= ChangeMovementState;
    }
    
  #endregion
    
    #region Move
    private void LateUpdate()
    {
        GravityHandling();
    }

    public void MoveCharacter(Vector3 moveDirection)
    {
        m_VelocityDirection.x = moveDirection.x;
        m_VelocityDirection.z = moveDirection.z;
        m_VelocityDirection.Normalize();
        var direction = m_VelocityDirection * m_MoveSpeed;
       
        var valueToMove = (m_VelocityDirection.x + m_VelocityDirection.z) /2;
       
        m_CharacterController.Move(direction * Time.deltaTime);
       // var maxSlimeLevel = _levelManager.AllSlimePlayer.Aggregate((r, x) 
           // => r.SlimeDatas.level > x.SlimeDatas.level ? r : x).SlimeDatas.level;
        
    }

    public void RotateCharacter(Vector3 moveDirection)
    {
        if (Vector3.Angle(transform.forward, moveDirection) > 0)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, m_RotationSpeed*Time.deltaTime, 0);
           // Vector3 newDirection = Vector3.Lerp(transform.forward, moveDirection, .1f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        // if (ccVelocity != Vector3.zero && forwardVelocityThreshold)
        // {
        //     toRotation = Quaternion.LookRotation(ccVelocity, Vector3.up);
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
        // }
    }

    private void GravityHandling()
    {
        if (!m_CharacterController.isGrounded)
            m_VelocityDirection.y -= m_GravityForce * Time.deltaTime;
        else
            m_VelocityDirection.y = -0.5f;
    }

    public void BlockMovement()
    {
       // InputVariables.MoveSpeed = 0;
        //m_RotationSpeed = 0;
    }

    private void ChangeMovementState(bool state)
    {
        m_MoveSpeed = state ? 0 : InputVariables.MoveSpeed;
        m_RotationSpeed = state ? 0 : InputVariables.RotationSpeed;
    }
    #endregion
   
    
}
