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
        _skillTypes = SkillTypes.HeartPlayer;
        Data = new SkillData
        {
            DescSkill = "Can hasarını %100 iyileştirir.",
            SpriteSkill = Resources.Load<Sprite>("HeartSkill")
        };
    }

 
}
