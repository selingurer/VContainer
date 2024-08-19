
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameDataObject", order = 1)]
public class GameData :ScriptableObject
{
    public int MaxPoolSize { get => 50; private set { } }
}
