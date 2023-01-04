using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CopySlimeManager : Singleton<CopySlimeManager>
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    [SerializeField] private GameObject m_CopyContainer;
    [SerializeField] private Transform aim;
    [SerializeField] private Vector3 offset;
    public void MergeSlimeCopy(MergeSlime slime)
    {
        var copy = Instantiate(slime,slime.transform.position,Quaternion.identity,m_CopyContainer.transform);
        copy.m_Parts.layer = 5;
        foreach (Transform child in copy.m_Parts.transform)
            child.gameObject.layer = 5;
        Destroy(copy.GetComponent<Collider>());
        copy.transform.eulerAngles = GameplayVariables.SlimeRotateError;
        copy.transform.localScale = slime.transform.localScale * 2.5f;
        //gameObject.layer uses only integers, but we can turn a layer name into a layer integer using LayerMask.NameToLayer()
        Sequence newSequence = DOTween.Sequence();
        newSequence.Append(copy.transform.DOMove(copy.transform.position+ offset,.8f));
        newSequence.Append(copy.transform.DOMove(aim.transform.position, .5f).SetEase(Ease.Linear));
        newSequence.Append(copy.transform.DOScale(0, .2f))
            .OnComplete(()=>Destroy(copy.gameObject));
    }
    
}
