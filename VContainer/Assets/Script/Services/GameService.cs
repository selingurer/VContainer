using Assets.Script.Services;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameService : IStartable
{
    private ILevelService _levelService;
    private ObjectPool<Enemy> _enemyPool;
    private Player _player;
    private EnemyService _enemyService;

    [Inject]
    private void Construct(ILevelService service, IObjectResolver resolver, Player player, ObjectPool<Enemy> enemyPool,EnemyService enemyService)
    {
        _levelService = service;
        _player = player;
        _enemyPool = enemyPool;
        _enemyService = enemyService;
    }

    void IStartable.Start()
    {
        _levelService.SetLevel(1);
        CreateEnemy();
    }

    public void CreateEnemy()
    {
        List<Vector2> vectorList = new List<Vector2>();
        for (int i = 0; i < _levelService.GetInitialPoolSize(); i++)
        {
            int x = Random.Range(-90, 90);
            int xV = x == _player.transform.position.x ? 30 : x;
            int y = Random.Range(-90, 90);
            int yV = y == _player.transform.position.y ? 30 : y;
            vectorList.Add(new Vector2(_player.transform.position.x + xV, _player.transform.position.y + yV));
        }
        for (int i = 0; i < _levelService.GetInitialPoolSize(); i++)
        {
            var obj = _enemyPool.Get();
            obj.gameObject.transform.position = vectorList[i];
            _enemyService._ActiveEnemyList.Add(obj);
        }
    }

}

