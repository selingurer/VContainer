using UnityEngine;

public class SkillSpeed : ISkillSpeed
{
    public float SpeedBost { get =>1.3f; }

    public void SetSkillSpeed(CharacterData data)
    {
        data.Speed *= SpeedBost; 
    }
}
