
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public interface ISkillShield
{
    UniTask SetSkillShield(PlayerData dataPlayer, Transform transform, CancellationToken cancellationToken);
}

