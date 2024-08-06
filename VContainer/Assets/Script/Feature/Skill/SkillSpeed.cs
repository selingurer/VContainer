using UnityEngine;
using VContainer.Unity;

public class SkillSpeed : Skill, IStartable
{
    private float _speedBoost = 1.3f;
    
    protected override void SetSkill()
    {
        Debug.LogError(_player._speed.GetSpeed());
        base.SetSkill();
        _player._speed.SetSpeed(_player._speed.GetSpeed() * _speedBoost);
        Debug.LogError(_player._speed.GetSpeed());
    }

    public void Start()
    {
        Data = new SkillData
        {
            DescSkill = "Hýzýný %30 arttýrýr.",
            SpriteSkill = Resources.Load<Sprite>("SpeedSkill")
        };
        
    }
}
