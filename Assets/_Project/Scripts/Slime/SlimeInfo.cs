using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SlimeInfo
{
    public SlimeType e_SlimeMainType;
    public SlimeType e_SlimeMergedType;
    public int SlimeMainIndex;
    public int SlimeMergedIndex;
    public bool isMerged;
    public Color GradientColor;
    public float IncomeTimePassed;
    
    public int GridID;
    public int SlotID;
    
    public SlimeInfo(SlimeInfo newInfo)
    {
        e_SlimeMainType = newInfo.e_SlimeMainType;
        e_SlimeMergedType = newInfo.e_SlimeMergedType;
        SlimeMainIndex = newInfo.SlimeMainIndex;
        SlimeMergedIndex = newInfo.SlimeMergedIndex;
        isMerged = newInfo.isMerged;
        GradientColor = newInfo.GradientColor;
        IncomeTimePassed = newInfo.IncomeTimePassed;

        GridID = newInfo.GridID;
        SlotID = newInfo.SlotID;
    }
    public SlimeInfo()
    {
   
    }
    public override bool Equals(object obj)
    {
        var slime = obj as SlimeInfo;
        return SlimeMainIndex == slime.SlimeMainIndex && 
               SlimeMergedIndex == slime.SlimeMergedIndex && 
               isMerged == slime.isMerged &&
               e_SlimeMainType == slime.e_SlimeMainType &&
               e_SlimeMergedType == slime.e_SlimeMergedType;
    }
}
public enum SlimeType
{
    Forest = 0,
    Winter = 1,
    Desert = 2,
    Coniferous = 3,
}


