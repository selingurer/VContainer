
using System;
using UnityEngine;

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