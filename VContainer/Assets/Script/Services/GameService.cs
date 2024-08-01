using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameService : IStartable
{
    private Enemy _objEnemy;
    private Transform _enemyTransform;
    private IObjectResolver _resolver;
    private ILevelService _levelService;
    private ObjectPool<Enemy> _enemyPool;
    public int initialEnemyPoolSize = 10;

    [Inject]
    private void Construct(ILevelService service, IObjectResolver resolver, Enemy objEnemy, Transform enemyTransform )
    {
        _levelService = service;
        _resolver = resolver;
        _objEnemy = objEnemy;
        _enemyTransform = enemyTransform;
    }

    void IStartable.Start()
    {
        _levelService.SetLevel(1);
        _enemyPool = new ObjectPool<Enemy>(_objEnemy, initialEnemyPoolSize, _resolver, _enemyTransform);
    }
}

