
public class SkillHealth : ISkillHealth
{
    public void SetSkillHealth(PlayerData data)
    {
        data.Health = data.FirstHealth;
    }
}
