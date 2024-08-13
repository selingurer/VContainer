using UnityEngine;

//[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerDataObject", order = 1)]
public class PlayerData : CharacterData
{
    public int ExperienceValue = 0;
    public int TotalExperienceValue = 0;
    public float FirstHealth = 200;
    public bool Sheild = false;
}

public class CharacterData
{
    public int Attack = 100;
    public float Speed = 4.5f ;
    public float Health = 200;
}