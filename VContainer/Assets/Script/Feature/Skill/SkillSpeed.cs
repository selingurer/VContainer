using UnityEngine;
using VContainer.Unity;

public class SkillSpeed : Skill, IStartable
{
    private float _speedBoost = 1.3f;
    
    protected override void SetSkill()
    {
        base.SetSkill();
        _player.SetSpeed(_player.Speed * _speedBoost);
    }

    public void Start()
    {
        Data = new SkillData
        {
            DescSkill = "H�z�n� %30 artt�r�r.",
            SpriteSkill = Resources.Load<Sprite>("SpeedSkill")
        };
        
    }
}
