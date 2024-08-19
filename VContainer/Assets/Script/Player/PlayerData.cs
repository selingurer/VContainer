using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDataObject", order = 1)]
public class PlayerData : CharacterData
{
    public int ExperienceValue = 0;
    public int TotalExperienceValue = 0;
    public float FirstHealth = 200;
    public bool Shield = false;
    private int DefaultExperienceValue = 0;
    private int DefaultTotalExperienceValue = 0;
    private float DefaultFirstHealth = 200;
    private bool DefaultShield = false;

    public void ResetToPlayerData()
    {
        ExperienceValue = DefaultExperienceValue;
        TotalExperienceValue = DefaultTotalExperienceValue;
        FirstHealth = DefaultFirstHealth;
        Shield = DefaultShield;

    }
}

public class CharacterData : ScriptableObject
{
    public int Attack = 100;
    public float Speed = 4.5f;
    public float Health = 200;
    private int DefaultAttack = 100;
    private float DefaultSpeed = 4.5f;
    private float DefaultHealth = 200;
    public void ResetToData()
    {
        Attack = DefaultAttack;
        Speed = DefaultSpeed;
        Health = DefaultHealth;
    }
}
