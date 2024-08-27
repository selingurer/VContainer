using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBulletSpawnerService
{
    void GetBullet(ITargetable target, Component owner , float attackValue);
}