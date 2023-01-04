using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CarAnimation : Singleton<CarAnimation>
{
    [SerializeField] private GameObject[] m_Wheels;
    
    [SerializeField] private GameObject coin;
    [SerializeField] private Transform coinStart;
    [SerializeField] private Transform coinEnd;
    [SerializeField] private Transform slimePos;
    [SerializeField] private Transform carStartPos;
    [SerializeField] private Transform carArrivePos;
    [SerializeField] private Transform carEndPos;

    [Button]
    public void CarArrive()
    {
    //     transform.DOMove(carArrivePos.position,1f);
    //     foreach (var wheel in m_Wheels)
    //         wheel.transform.DORotate(new Vector3(360,0,0), 1f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
     }
    [Button]
    public void CarLeave()
    {
        transform.DOMove(carEndPos.position,3f);
    }
    [Button]
    public void SlimeToCar(GameObject slime)
    {
        Sequence newSequence = DOTween.Sequence();
     
        slime.transform.parent = transform;
        newSequence.Append(slime.transform.DOLocalJump(slimePos.localPosition, 2f, 1, 2f));
        newSequence.AppendCallback(CoinFall);
        newSequence.AppendCallback(CarLeave);
        newSequence.AppendCallback(()=>
        {
            CameraManager.Instance.ChangeCamera(Cameratype.Creative03_cryMoney);
            var player = FindObjectOfType<Character>();
            player.CryMoney();
        });
        
    }

    public void CoinFall()
    {
        coin.transform.DOScale(new Vector3(3,3,1.5f),1.3f);
        coin.transform.DOJump(coinEnd.position,2f,1,1.3f)
            .OnComplete(()=> coin.transform.DORotate(new Vector3(0,720,0), 5f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear));
    }

}
