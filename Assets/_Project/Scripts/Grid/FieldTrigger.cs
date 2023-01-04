using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class FieldTrigger : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    [SerializeField] private GameObject m_FieldBuyZone;
    [SerializeField] private MeshRenderer m_MeshRenderer;

    public static Action<bool> OnCanDrag;
    public static Action<bool,int> OnEnterDropZone;
    public static Action<int> OnFieldClose;
    public Cameratype Cameratype;
    
    public int DefaultPrice;
    public int GridID;
    public string Id;
    public bool IsOpened;
    
    private const string IsSlimeDeleted = "IsSlimeDeleted";
    private const string IsSlimeAdded = "IsSlimeAdded";
    
    [Button]
    private void setRefs()
    {
        GridID = transform.parent.GetComponentInChildren<GridContainer>().GridID;
        if(GridID!=0)
            m_FieldBuyZone = transform.parent.parent.GetComponentInChildren<BuyZone>().gameObject;
        m_MeshRenderer = transform.parent.FindDeepChild<MeshRenderer>("zone");
        Id = $"{m_FieldBuyZone.name}";
        
    }
    private void OnValidate()
    {
      //  m_Collider = GetComponent<Collider>();
   //     m_CharacterMovement = FindObjectOfType<CharacterMovement>();  
        //setRefs();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!IsOpened)
            return;
        if (other.TryGetComponent(out Character character))
        {
            CameraManager.Instance.ChangeCamera(Cameratype);
            Debug.Log("triggered: " + Cameratype);
            OnCanDrag?.Invoke(true);
            OnEnterDropZone.Invoke(true, GridID);
            CarAnimation.Instance.CarArrive();
            if (GridID == 0)
                TutorialManager.Instance.OnStartGridTutorTimer.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(!IsOpened)
            return;
        if (other.TryGetComponent(out Character character))
        {
            CameraManager.Instance.ChangeCamera(Cameratype.PlayerCamera);
            OnCanDrag?.Invoke(false);
            OnEnterDropZone.Invoke(false, GridID);

            TutorialManager.Instance.OnAddTutorDisabled.Invoke();
            TutorialManager.Instance.OnDeleteTutorDisabled.Invoke();
            TutorialManager.Instance.OnMergeTutorDisabled.Invoke();
            Character.currentFieldID = 100;
            TutorialManager.Instance.ResetTimer();
        }
    }

    public void ChangeCamera()
    {
        CameraManager.Instance.ChangeCamera(Cameratype);
        OnCanDrag?.Invoke(true);
        OnEnterDropZone.Invoke(true, GridID);
    }

    public void SetMaterial()
    {
        if (IsOpened)
        {
            m_MeshRenderer.material = GameplayVariables.FieldOpenMaterial;
            return;
        }
        m_MeshRenderer.material = GameplayVariables.FieldCloseMaterial;
    }
}
