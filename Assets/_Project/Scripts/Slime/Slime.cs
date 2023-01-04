using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Slime : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    [SerializeField] private SlimeInfo m_SlimeInfo;
    [SerializeField] private SkinnedMeshRenderer[] m_Renderers;
    [SerializeField] private SlimeStates m_SlimeStates;
    [SerializeField] private SlimeBlobAnimation m_SlimeBlobAnimation;
    public SlimeCatchBar m_SlimeCatchBar;
    [SerializeField] private Collider m_Collider;
    [SerializeField] private float m_MaxCatchTime;
    [SerializeField] private float m_CurrentCatchTime;
    [SerializeField] private float m_FillCatchStep;
    [SerializeField] private float m_ReduceCatchStep;
    
    [SerializeField] private List<GameObject> m_SlimeParts;
    [SerializeField] private List<Color> m_SlimeColors;

    [SerializeField] private bool isForCreative;
    private IEnumerator m_Reducing;

    private const string c_Color1 = "Color_984f38cfa15340baa777476ede88ef22";
    private const string c_Color2 = "Color_c3a4016b5cd24d5e9964807f243a0d05";

    private bool isCatched;
    public bool m_InFieldOfView;

    #region Editor
    private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
        // m_Collider = GetComponent<Collider>();
        // m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_SlimeCatchBar = GetComponentInChildren<SlimeCatchBar>();
        m_SlimeBlobAnimation = GetComponentInChildren<SlimeBlobAnimation>();
        m_SlimeStates = GetComponent<SlimeStates>();
        m_MaxCatchTime = GameplayVariables.MaxCatchTime;
        m_FillCatchStep = GameplayVariables.FillCatchStep;
        m_ReduceCatchStep = GameplayVariables.ReduceCatchStep;
    }
    #endregion
    
    #region Init
    private void Start()
    {
        m_CurrentCatchTime = m_MaxCatchTime;
        if (isForCreative)
            InitSlime(m_SlimeInfo);
    }

    private void OnEnable()
    {
        OnLevelLoaded();
    }

    private void OnDisable()
    {
        //GameManager.onLevelLoaded -= OnLevelLoaded;
    }
    #endregion

    #region Callbacks
    private void OnLevelLoaded()
    {

    }
    #endregion

    #region Specific
    
    public void InitSlime(SlimeInfo slimeInfo)
    {
        //transform.eulerAngles = GameplayVariables.SlimeRotateError;
        transform.eulerAngles = new Vector3(180,180,180);
        m_SlimeInfo = slimeInfo;
        var indexToActivate = (int)m_SlimeInfo.e_SlimeMainType * 3 + m_SlimeInfo.SlimeMainIndex;
        if(m_SlimeParts[indexToActivate]!=null)
            m_SlimeParts[indexToActivate].SetActive(true);
        
        foreach (var meshRenderer in m_Renderers)
        {
            var tmp = new List<Material>( meshRenderer.materials);
            tmp[0].SetColor(c_Color1,m_SlimeColors[m_SlimeInfo.SlimeMainIndex]);
            tmp[0].SetColor(c_Color2,m_SlimeColors[m_SlimeInfo.SlimeMainIndex]);
            meshRenderer.materials = tmp.ToArray();
        }
        m_SlimeBlobAnimation.m_BlobColor = m_SlimeColors[m_SlimeInfo.SlimeMainIndex];
        
        //var startIndex = (int)slimeInfo.e_SlimeType * 3;
        // for (int i = startIndex; i < startIndex+3; i++)
        //     if(m_SlimeInfo.SlimeActiveIndex == i/3)
        //         m_SlimeParts[i].SetActive(true);
    }

    public void OnCatchStateUpdated(bool state)
    {
        if(isCatched)
            return;
        m_InFieldOfView = state;
        m_SlimeCatchBar.FadeInOut(state);
        StartCoroutine(updateCatchTimer());
        if (state) {
            m_SlimeStates.SetSlowSpeed();
            m_SlimeStates.displacementState(true);
            return;
        }
        m_SlimeStates.SetRunSpeed();
        m_SlimeStates.displacementState(false);
    }
    private IEnumerator updateCatchTimer()
    {
        float catchStep = m_InFieldOfView ? m_ReduceCatchStep : m_FillCatchStep;
        while (m_CurrentCatchTime > 0 && m_CurrentCatchTime <= m_MaxCatchTime)
        {
            m_CurrentCatchTime += catchStep;
            m_SlimeCatchBar.UpdateCatchBar(m_CurrentCatchTime);
            yield return new WaitForSeconds(0.05f);
        }
        
        if (m_CurrentCatchTime > m_MaxCatchTime)
            m_CurrentCatchTime = m_MaxCatchTime;
        
        if(m_CurrentCatchTime<=0)
            CatchSlime();
    }
    
    private void CatchSlime()
    {
        var player = FindObjectOfType<Character>();
        player.needToRotate = false;
        player.m_CurrentTarget = null;
        var fov = FindObjectOfType<FieldOfView>();
        fov.DeleteTarget();
        
        Debug.Log(player.m_CurrentTarget);
       // Debug.Break();
       // player.TornadoFadeOut();
        isCatched = true;
        m_Collider.isTrigger = true;
        m_Collider.enabled = false;
        m_SlimeBlobAnimation.isCatched = true;
        transform.DOScale(0, .5f);
        transform.DOMove(m_SlimeStates.Character.WeaponMuzzle.position, .5f)
            .OnComplete(onAnimEnded);
    }

    private void onAnimEnded()
    {
        BagPack.OnSlimeAdded.Invoke(m_SlimeInfo);
        SoundManager.Instance.PlaySlimeCatch();
        FieldOfView.OnSlimeCatched.Invoke();
        Destroy(gameObject);
    }

    #endregion
    
    #region Physics
    
    #endregion
}