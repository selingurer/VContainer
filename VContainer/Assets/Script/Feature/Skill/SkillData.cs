
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillDataObject", order = 1)]

public class SkillDataList : ScriptableObject
{
    public List<SkillData> data;
}
public enum SkillTypes
{
    Speed = 0,
    Heart = 1,
    Shield = 2,
}
public class SkillData
{
    public SkillTypes SkillType;
    public Sprite SpriteSkill;
    public string DescSkill;
    public Action SkillAction;
}