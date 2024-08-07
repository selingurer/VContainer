using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public class SkillShield : Skill, IStartable
{
    int _delayTime = 10000;
    protected async override void SetSkill()
    {
        base.SetSkill();
        if (_player._shield)
            return;
        _player._shield = true;
        _gameUIPanel.CreateSkillGameObject();
        await UniTask.Delay(_delayTime);
        _player._shield = false;
    }

    void IStartable.Start()
    {
        _skillTypes = SkillTypes.ShieldPlayer;
        Data = new SkillData
        {
            DescSkill = "Koruma Kalkaný saðlar. 10 saniye boyunca hasar almazsýn.",
            SpriteSkill = Resources.Load<Sprite>("HeartPlusIcon")
        };
    }
}

