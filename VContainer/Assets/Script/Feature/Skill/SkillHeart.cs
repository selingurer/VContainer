using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class SkillHeart : Skill, IStartable
{

    protected override void SetSkill()
    {
        base.SetSkill();
        _player.Healt =_player.FirstHealt;
    }

    void IStartable.Start()
    {
        _skillTypes = SkillTypes.HeartPlayer;
        Data = new SkillData
        {
            DescSkill = "Can hasar�n� %100 iyile�tirir.",
            SpriteSkill = Resources.Load<Sprite>("HeartSkill")
        };
    }

 
}
