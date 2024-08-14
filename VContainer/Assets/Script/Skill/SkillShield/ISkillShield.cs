
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISkillShield
{
    UniTask SetSkillShield(PlayerData dataPlayer, Transform transform);
}

