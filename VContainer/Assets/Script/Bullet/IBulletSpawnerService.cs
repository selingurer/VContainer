using UnityEngine;

public interface IBulletSpawnerService
{
    void GetBullet(ITargetable target, Component owner, Vector3 pos, float attackValue);
}