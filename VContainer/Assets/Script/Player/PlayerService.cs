using Assets.Script.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class PlayerService
{
    private int _cooldownTime = 1000;
    private bool _canDamage = true;
    private ObjectPool<Bullet> _bulletPool;
    private EnemyService _enemyService;
    private int _attack; 
    private PlayerData _playerData;
    public async UniTaskVoid Shoot(Vector3 pos)
    {
        if (!_canDamage) return;
        Enemy enemy = _enemyService.GetClosestEnemy(pos);
        if (enemy != null)
        {
            _canDamage = false;

            var obj = _bulletPool.Get();
            obj.transform.position = pos;
            obj.Target(enemy);
            obj._attackValue = _attack;
            obj.SetObjectPool(_bulletPool);

            await UniTask.Delay(_cooldownTime);

            _canDamage = true;
        }
    }
    public void SetService(ObjectPool<Bullet> objPoolBullet, EnemyService enemyService)
    {
        _bulletPool = objPoolBullet;
        _enemyService = enemyService;
    }
    public void SetAttack(int attackValue)
    {
        _attack = attackValue;
    }
    public void Damage(float value)
    {

    }
}
