using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngrySlime : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private const string c_EatSlime = "EatSlime";
    public static Action<MergeSlime> OnSlimeEaten;
    private int counterToCry;
    private void OnEnable()
    {
        OnSlimeEaten += EatSlime;
    }
    private void OnDisable()
    {
        OnSlimeEaten -= EatSlime;
    }
    private void EatSlime(MergeSlime slime)
    {
        
        this.WaitRealSecond(.8f,(() => animator.SetTrigger(c_EatSlime)));
        slime.GetsEatenAnim(transform.position);
    }
    public void CryCharacter()
    {
        if (counterToCry != 1) {
            counterToCry++;
            CameraManager.Instance.ChangeCamera(Cameratype.PlayerCamera);
            return;     
        }
       
        var player = FindObjectOfType<Character>();
        CameraManager.Instance.ChangeCamera(Cameratype.Creative02_ringCry);
        player.WaitRealSecond(.6f,()=>player.Cry());
    }
}
