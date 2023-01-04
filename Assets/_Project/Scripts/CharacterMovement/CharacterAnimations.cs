using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
public class CharacterAnimations: MonoBehaviour
{
  //  [SerializeField] private DynamicJoystick m_Joystick;
    private InputManager m_InputManager => InputManager.Instance;
    [SerializeField] private BagPackUI m_BagPackUI;
    [SerializeField] private Animator m_Animator;
    private const string c_Move = "Move";
    private const string c_GetsEaten = "GetsEaten";
    private const string c_CryMoney = "CryMoney";

    [Button]
    private void OnValidate() => setRefs();
    private void setRefs()
    {
        m_Animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        float direction = (Mathf.Abs(m_InputManager.JoystickDirection.x)+Mathf.Abs(m_InputManager.JoystickDirection.y))/2;
        m_Animator.SetFloat(c_Move,direction);
    }
    
    public void GetsEaten(Vector3 position)
    {
        m_Animator.SetTrigger(c_GetsEaten);
        
        var player = GetComponent<Character>();
        player.GetComponent<CharacterController>().enabled = false;
        Destroy(player.GetComponent<Rigidbody>());
        Sequence newSequence = DOTween.Sequence();
        newSequence.AppendInterval(1f);
        newSequence.Append(transform.DOScale(Vector3.zero, 2f));
        newSequence.Join(transform.DOMove(position, 2f));

        m_BagPackUI.offText();
    }

    public void CryMoney()
    {
        this.WaitRealSecond(1f,
            () =>
            {
                
            });

        // this.WaitRealSecond(1f,
        //     ()=>m_Animator.SetTrigger(c_CryMoney));
    }
    
}
