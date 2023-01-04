using System;
using UnityEngine;
using UnityEngine.Serialization;

public class JoystickForMovement : MonoBehaviour
{
    [SerializeField] private GameObject m_Joystick;
    [SerializeField] private CharacterMovement m_Movement;

    [SerializeField] private bool m_CanMove;
    private void onLevelStarted()
    {
        m_Movement = GetComponent<CharacterMovement>();
        EnableJoystick();
        m_CanMove = true;
    }
    private void OnEnable()
    {
        GameManager.onLevelStarted += onLevelStarted;
        DragDropSlime.OnDrag += CanMove;
    }
   
    private void OnDisable()
    {
        GameManager.onLevelStarted -= onLevelStarted;
        DragDropSlime.OnDrag -= CanMove;
    }
    

    #region Specific
    private void EnableJoystick()
    { 
        m_Joystick.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(!m_CanMove)
            return;
        //    var sideForce = -(m_Joystick.Horizontal);
        //    var forwardForce = -(m_Joystick.Vertical);
        var direction = InputManager.Instance.JoystickDirection;
        m_Movement.MoveCharacter(new Vector3(-direction.x, 0, -direction.y));

        if (direction.x != 0 || direction.y != 0)
        {
            m_Movement.RotateCharacter(new Vector3(-direction.x, 0, -direction.y));
        }
    }
    #endregion
    private void CanMove(bool state)
    {
        m_CanMove = !state;
        m_Joystick.gameObject.SetActive(!state);
    }
 

}