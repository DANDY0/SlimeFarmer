using System;
using UnityEngine;

public class GameConfig : GameConfigBase
{
    public InputVariablesEditor Input = new InputVariablesEditor();
    public AudioVariablesEditor Audio = new AudioVariablesEditor();
    public CollectableVariablesEditor CollectableData = new CollectableVariablesEditor();
    public PopupVariablesEditor Popup = new PopupVariablesEditor();
    public GameplayVariablesEditor Gameplay = new GameplayVariablesEditor();
}

[Serializable]
public class InputVariablesEditor : GameConfigBase.InputVariablesEditorBase
{
    //
    public float MoveSpeed;
    public float RotationSpeed;
    public float GravityForce;
    public float ValueToMove;

}

[Serializable]
public class AudioVariablesEditor : GameConfigBase.AudioVariablesEditorBase
{
    //
}

[Serializable]
public class CollectableVariablesEditor : GameConfigBase.CollectableVariablesEditorBase
{
    //
}
[Serializable]
public class GameplayVariablesEditor
{
    [Header("Player")]
    public float FieldAnimDuration;
    [Header("Slimes")]
    public float MaxCatchTime;
    public float FillCatchStep;
    public float ReduceCatchStep;
    public float SlimeBarAnimDuration;
    [Header("Bag")]
    public int MaxBagCapacity;
    [Header("Drag")]
    public Vector3 CardOffset;
    public float CardMaxUpValue;
    public float MaxRaycastDistance;
    public float SmoothAnimValue;
    //  public float m_AnimSpeedStep => m_GamePlayParameters.AnimSpeedStep;
    public float AnimNewSlotSpeed;
    public float AnimSpeedParent;
    [Header("Camera")]
    public float TransitionDuration;
    [Header("GridSlimes")]
    public Vector3 GridSlimeUpOffset;
    public Vector3 FirstScaleValue;
    public Vector3 SecondScaleValue;
    public Vector3 SlimeRotateError;
    public Vector3 SlimeRotateErrorRing;
    public float TimeBetweenSlimeDrop;
    public float DropSlimeDelay;
    [Header("Biom")]
    public float TimeToResetBiom;
    public float TutorailDragTime;
    public float TutorialGridTime;
    [Header("Shop")]
    public int[] FieldDefaultPrices;
    public int[] PortalDefaultPrices;
    public Material FieldCloseMaterial;
    public Material FieldOpenMaterial;
    public int[] MaxDiscoveredSlimesCount;
}

[Serializable]
public class PopupVariablesEditor: GameConfigBase.PopupVariablesEditorBase
{
    public SettingColor SettingIconColor;
    public WinPopupData WinPopup;

    [Serializable]
    public class SettingColor
    {
        public Color OnIconColor = new Color();
        public Color OffIconColor = new Color();
    }

    [Serializable]
    public class WinPopupData
    {
        public Sprite FullStar;
        public Sprite EmptySprite;
    }
}