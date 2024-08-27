using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class BulletSpawnerService : IBulletSpawnerService
{
    [Inject] private ObjectPool<BulletView> _bulletPool;

    public void BulletReturnToPool(BulletView bullet)
    {
        _bulletPool.ReturnToPool(bullet);
    }
    public void GetBullet(ITargetable target, Component owner, float attackValue)
    {
        var obj = _bulletPool.Get();
        obj.transform.position = owner.transform.position;
        obj.SetTarget(target, owner);
        obj._attackValue = attackValue;
        obj.ReturnToPoolBulletAction += OnReturnToPool;
    }

    private void OnReturnToPool(BulletView bullet)
    {
        BulletReturnToPool(bullet);
        bullet.ReturnToPoolBulletAction -= OnReturnToPool;
    }
}
