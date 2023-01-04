using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;
    [SerializeField] private GridContainer m_GridContainer;
    public float TimeToReset => GameplayVariables.TutorialGridTime;
    public float TimePassed;
    
    public Action OnDragTutorEnabled;
    public Action OnDragTutorDisabled;
    public Action OnAddTutorEnabled;
    public Action OnAddTutorDisabled;
    public Action OnDeleteTutorEnabled;
    public Action OnDeleteTutorDisabled;
    public Action OnMergeTutorEnabled;
    public Action OnMergeTutorDisabled;
    
    public Action OnStartGridTutorTimer;

    private const string IsSlimeDeleted = "IsSlimeDeleted";
    private const string IsSlimeAdded = "IsSlimeAdded";
    private const string IsSlimeMerged = "IsSlimeMerged";

    public bool isInField;

    private void OnEnable()
    {
        OnStartGridTutorTimer += StartTimer;
        OnAddTutorDisabled += ResetTimer;
        OnDeleteTutorDisabled += ResetTimer;
        OnMergeTutorDisabled += ResetTimer;
    }
    private void OnDisable()
    {
        OnStartGridTutorTimer -= StartTimer;
        OnAddTutorDisabled -= ResetTimer;
        OnDeleteTutorDisabled -= ResetTimer;
        OnMergeTutorDisabled -= ResetTimer;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnDragTutorDisabled.Invoke();
        }
    }

    private void StartTimer()
    {
        StartCoroutine(StartTimerCoroutine());
    }
        
    private IEnumerator StartTimerCoroutine()
    {
        TimePassed = 0;
        isInField = true;
        while (TimePassed < TimeToReset)
        {
            TimePassed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        
        if (!m_GridContainer.IsFirstTwoSlotsFull())
           yield break;

        if (PlayerPrefs.GetInt(IsSlimeMerged) == 0)
        {
            OnMergeTutorEnabled.Invoke();
            yield break;
        }
        if (PlayerPrefs.GetInt(IsSlimeAdded) == 0)
        {
            OnAddTutorEnabled.Invoke();
            yield break;
        }
        if (PlayerPrefs.GetInt(IsSlimeDeleted) == 0)
            OnDeleteTutorEnabled.Invoke();
    }
    
    public void ResetTimer()
    {
        TimePassed = 0;
        isInField = false;
        StopAllCoroutines();
    }
}
