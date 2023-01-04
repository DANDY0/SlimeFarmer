using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Extensions;
using GameAnalyticsSDK.Setup;
using Sirenix.OdinInspector;
using UnityEngine;


public class MergeSlime : MonoBehaviour
{
    private GameplayVariablesEditor GameplayVariables => GameConfig.Instance.Gameplay;

    [SerializeField] private Animator m_Animator;
    [SerializeField] private Animator m_AngryAnimator;
    [SerializeField] private GameObject NewAnimationContainer;
    [SerializeField] private SkinnedMeshRenderer m_Displacement;
    [SerializeField] private DragDropSlime m_DragDropSlime;
    [SerializeField] private SlimeIncome m_SlimeIncome;
    [SerializeField] private ParticleSystem m_Confetti;
    public SlimeInfo m_SlimeInfo;
    [SerializeField] private SkinnedMeshRenderer[] m_Renderers;
    [SerializeField] private Material m_DefaultMaterial;
    [SerializeField] private Material m_DeleteMaterial;
    [SerializeField] private Material m_AddMaterial;
    [SerializeField] private Material m_MergeMaterial;
    [SerializeField] private List<GameObject> m_SlimeParts;

    [Header("Creatives")]
    [SerializeField] private GameObject m_AngrySlime;
    [SerializeField] private bool isCreative;
    [SerializeField] private bool isNeedToEatPlayer;
    [SerializeField] private bool isJumpInCar;
    [SerializeField] private GameObject m_MoneyBurst;
    

    public GameObject m_Parts;
    public List<Color> m_SlimeColors;
    
    private const string c_Color1 = "Color_984f38cfa15340baa777476ede88ef22";
    private const string c_Color2 = "Color_c3a4016b5cd24d5e9964807f243a0d05";
    private const string c_Displacement = "_Displacement";
    private const string IsSlimeDeleted = "IsSlimeDeleted";
    private const string IsSlimeAdded = "IsSlimeAdded";
    private const string c_EatPlayer = "EatPlayer";
    private int m_SlimeTypesCount = 3;
    private float mergeAnimDuration = .8f;
    [SerializeField] private float minDisplacementValue;
    [SerializeField] private float maxDisplacementValue;
    [SerializeField] private string c_GetsEaten = "GetsEaten";

    #region Editor
        private void OnValidate()
    {
        SetRefs();
    }

    [Button]
    private void SetRefs()
    {
//        m_SlimeInfo = GetComponent<SlimeInfo>();

        m_SlimeIncome = GetComponent<SlimeIncome>();
        m_DragDropSlime = GetComponent<DragDropSlime>();
        m_Parts = transform.FindDeepChild<GameObject>("Parts");
        //    m_SlimeStates = GetComponent<SlimeStates>();
        //m_DefaultMaterial = m_MeshRenderer.sharedMaterial;
    }
    #endregion
    
    #region Init
    private void Start()
    {
        if (isCreative)
        {
            InitSlime(m_SlimeInfo);
        }
    }
    private void OnEnable()
    {
        //GridSlot.OnGridAction += CheckID;
        //m_Displacement.material.SetFloat(c_Displacement, minDisplacementValue);
    }
    private void OnDisable()
    {
       // GridSlot.OnGridAction -= CheckID;
    }
    #endregion

    #region Callbacks
    private void OnLevelLoaded()
    {
        
    }
    #endregion

    #region Specific

    #region Inits
    public void InitSlime(SlimeInfo slimeInfo)
    {
        m_SlimeInfo = new SlimeInfo(slimeInfo);
        transform.eulerAngles = GameplayVariables.SlimeRotateError;
        var indexToActivate = (int)m_SlimeInfo.e_SlimeMainType * 3 + m_SlimeInfo.SlimeMainIndex;
        if(m_SlimeParts[indexToActivate]!=null)
            m_SlimeParts[indexToActivate].SetActive(true);

        var tempDefaultMaterial = new Material(m_Renderers[0].materials[0]);
        tempDefaultMaterial.SetColor(c_Color1, m_SlimeColors[m_SlimeInfo.SlimeMainIndex]);
        tempDefaultMaterial.SetColor(c_Color2, m_SlimeColors[m_SlimeInfo.SlimeMainIndex]);
        
        m_DefaultMaterial = tempDefaultMaterial;
        
        foreach (var meshRenderer in m_Renderers)
        {
            var tmp = new List<Material>( meshRenderer.materials);
            tmp[0].SetColor(c_Color1,m_SlimeColors[m_SlimeInfo.SlimeMainIndex]);
            tmp[0].SetColor(c_Color2,m_SlimeColors[m_SlimeInfo.SlimeMainIndex]);
            meshRenderer.materials = tmp.ToArray();
        }
        m_SlimeIncome.TimeIncome(slimeInfo);
        m_DragDropSlime.isThisField = true;
    }
    public void MergeSlimeDrop(SlimeInfo slimeInfo)
    {
        m_SlimeInfo = new SlimeInfo(slimeInfo);
        
        var indexToActivateParent = (int)m_SlimeInfo.e_SlimeMainType * m_SlimeTypesCount + m_SlimeInfo.SlimeMainIndex;
        var indexToActivateMerged = (int)m_SlimeInfo.e_SlimeMergedType * m_SlimeTypesCount + m_SlimeInfo.SlimeMergedIndex;
        
        if(m_SlimeParts[indexToActivateParent]!=null)
            m_SlimeParts[indexToActivateParent].SetActive(true);
        if(m_SlimeParts[indexToActivateMerged]!=null)
            m_SlimeParts[indexToActivateMerged].SetActive(true);

        var tempDefaultMaterial = new Material(m_Renderers[0].materials[0]);
        tempDefaultMaterial.SetColor(c_Color1, m_SlimeColors[m_SlimeInfo.SlimeMainIndex]);
        tempDefaultMaterial.SetColor(c_Color2, slimeInfo.GradientColor);
        tempDefaultMaterial.SetFloat("_Rough", 0.6f);
        
        m_DefaultMaterial = tempDefaultMaterial;
        
        foreach (var meshRenderer in m_Renderers)
        {
            var tmp = new List<Material>( meshRenderer.materials);
            tmp[0].SetColor(c_Color1,m_SlimeColors[m_SlimeInfo.SlimeMainIndex]);
            tmp[0].SetColor(c_Color2,slimeInfo.GradientColor);
            meshRenderer.materials = tmp.ToArray();
        }
        m_SlimeIncome.TimeIncome(slimeInfo);
        m_DragDropSlime.isThisField = true;
        
//        AngrySlime.OnSlimeEaten.Invoke(this);
            //m_Animator.SetTrigger(c_GetsEaten);
    }
    
    public void GetsEatenAnim(Vector3 position)
    {
        Sequence newSequence = DOTween.Sequence();
        newSequence.AppendInterval(1f);
        newSequence.AppendCallback(()=>CameraManager.Instance.ChangeCamera(Cameratype.Creative02_ringEaten));
        newSequence.AppendInterval(.5f);
        newSequence.Append(transform.DOScale(Vector3.zero, 1f));
        newSequence.Join(transform.DOMove(position+ new Vector3(0,.2f,0), 1f))
            .OnComplete((() => Destroy(gameObject)));
    }
    public void MergeSlimeInit(int newIndex, Color secondColor, SlimeType mergeSlimeType)
    {
        m_SlimeInfo.SlimeMergedIndex = newIndex;
        m_SlimeInfo.e_SlimeMergedType = mergeSlimeType;
        m_SlimeInfo.GradientColor = secondColor;
        
        var indexToActivate =(int)m_SlimeInfo.e_SlimeMergedType * 3 + m_SlimeInfo.SlimeMergedIndex;
        
        if(m_SlimeParts[indexToActivate]!=null)
            m_SlimeParts[indexToActivate].SetActive(true);

        var tempDefaultMaterial = new Material(m_Renderers[0].materials[0]);
        tempDefaultMaterial.SetColor(c_Color2, secondColor);
        
        m_DefaultMaterial = tempDefaultMaterial;
        
        foreach (var meshRenderer in m_Renderers)
        {
            var tmp = new List<Material>( meshRenderer.materials);
            // tmp[0].SetColor(c_Color1,m_SlimeColors[m_SlimeInfo.SlimeParentIndex]);
            tmp[0].SetColor(c_Color2, secondColor);
            meshRenderer.materials = tmp.ToArray();
        }
        m_SlimeInfo.isMerged = true;
        m_SlimeInfo = new SlimeInfo(m_SlimeInfo);
        m_SlimeIncome.TimeIncome(m_SlimeInfo);
        IncomeManager.onSlimeMerged.Invoke(m_SlimeInfo, this);
        m_DragDropSlime.isThisField = true;
        DisplacementAnimation();

        if (isCreative && !isNeedToEatPlayer)
        {
            var money = Instantiate(m_MoneyBurst,transform.position, Quaternion.identity);
            money.transform.eulerAngles = new Vector3(-90, 0, 0);
        }
        if(isJumpInCar)
            CarAnimation.Instance.SlimeToCar(this.gameObject);
    }

  #endregion

    #region Animations
    public void SetDefaultFromDelete()
    {
        StopAllCoroutines();
        StartCoroutine(ColorFade(m_DeleteMaterial, m_DefaultMaterial));
    }
    public void SetDefaultFromAdd()
    {
        StopAllCoroutines();
        StartCoroutine(ColorFade(m_AddMaterial, m_DefaultMaterial));
    }
    public void SetDeleteColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorFade(m_DefaultMaterial, m_DeleteMaterial));
    }
    public void SetAddColor()
    {
        StopAllCoroutines();
        StartCoroutine(ColorFade(m_DefaultMaterial, m_AddMaterial));
    }
    #endregion
    private IEnumerator ColorFade(Material material1, Material material2)
    {
        float duration = .3f;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            foreach (var meshRenderer in m_Renderers)
                meshRenderer.material.Lerp(material1, material2, elapsedTime/duration);
            // m_MeshRenderer.material.Lerp(material1, material2, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public SlimeInfo GetSlimeInfo()
    {
        return m_SlimeInfo;
    }
    public void GetSlimeIncome(SlimeIncome slimeIncome)
    {
        m_SlimeIncome = slimeIncome;
        m_SlimeInfo.IncomeTimePassed = m_SlimeIncome.TimePassed;
    }
    public void SetIDs(int gridID, int slotID)
    {
        m_SlimeInfo.SlotID = slotID;
        m_SlimeInfo.GridID = gridID;
    }

    public void EnableNewAnimation()
    {
        NewAnimationContainer.SetActive(true);
        m_Confetti.Play();
    }

    public void DisplacementAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(DisplacementMerge());
    }
    private IEnumerator DisplacementMerge()
    {
        float elapsedTime = 0;

        while (elapsedTime < mergeAnimDuration)
        {
            m_Displacement.material.SetFloat(c_Displacement, elapsedTime/mergeAnimDuration * maxDisplacementValue);
            // m_MeshRenderer.material.Lerp(material1, material2, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        while (elapsedTime > 0)
        {
            m_Displacement.material.SetFloat(c_Displacement, elapsedTime/mergeAnimDuration * minDisplacementValue);
            // m_MeshRenderer.material.Lerp(material1, material2, elapsedTime/duration);
            elapsedTime -= Time.deltaTime;
            yield return null;
        } 
        //   m_Displacement.material.SetFloat(c_Displacement, minDisplacementValue);
        if (isNeedToEatPlayer)
        {
            var player = FindObjectOfType<Character>();
            m_AngrySlime.SetActive(true);
            m_Parts.SetActive(false);
            CameraManager.Instance.ChangeCamera(Cameratype.Creative01);
            // StartCoroutine(rotateSlime());
            m_SlimeIncome.enabled = false;
            m_AngrySlime.transform.DOLocalRotate(new Vector3(0,-90,0),1f,RotateMode.LocalAxisAdd)
                .OnComplete((() =>
                {
                    m_AngryAnimator.SetTrigger(c_EatPlayer);
                    player.m_CharacterAnimations.GetsEaten(transform.position);
                }));
        }
    }

    #endregion

    #region Physics
    #endregion
}

