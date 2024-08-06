using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class SkillHeart : Skill, IStartable
{

    protected override void SetSkill()
    {
        Debug.LogError(_player._speed.GetSpeed());
        base.SetSkill();
        _player._healt.SetHealt(_player._healt.FirstHealt, _player);
        Debug.LogError(_player._speed.GetSpeed());
    }

    void IStartable.Start()
    {
        Data = new SkillData
        {
            DescSkill = "Can hasarýný %100 iyileþtirir.",
            SpriteSkill = Resources.Load<Sprite>("HeartSkill")
        };
    }

 
}
