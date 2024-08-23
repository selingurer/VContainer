using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBulletSpawnerService
{
    void GetBullet(ITargetable<Component> target, Component owner , float attackValue);
}