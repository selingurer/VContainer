using Assets.Script.Services;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;


public class PlayerService
{
    [Inject] public PlayerData _playerData;
    public Action PlayerDead;
    public void Shoot(Vector3 pos, Enemy enemy)
    {
     //   Enemy enemy = _enemyService.GetClosestEnemy(pos);
        if (enemy != null)
        {
            var obj = _bulletPool.Get();
            obj.transform.position = pos;
            obj.Target(enemy);
            obj._attackValue = _playerData.Attack;
        }
    }

    public void Damage(float value)
    {
        _playerData.Health -= value;
        if (_playerData.Health < 0)
            PlayerDead?.Invoke();

    }
}
