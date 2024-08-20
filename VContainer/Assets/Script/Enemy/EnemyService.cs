using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Script.Services
{
    public class EnemyService : IDisposable, IStartable, IEnemyService
    {
        public List<EnemyView> _ActiveEnemyList = new List<EnemyView>();
        private ObjectPool<EnemyView> _enemyPool;
        private CancellationTokenSource cancellationTokenSource;
        public Action<Vector3> EnemyDead;
        [Inject]
        private void Construct(ObjectPool<EnemyView> objectPoolEnemy)
        {
            _enemyPool = objectPoolEnemy;
        }
        public async UniTask CreateEnemyAsync(Vector3 playerPos, int initialPoolSize)
        {
            try
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    CreateEnemy(playerPos, initialPoolSize);
                    await UniTask.Delay(20000, cancellationToken: cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Enemy creation task was canceled.");
            }
        }
        private void CreateEnemy(Vector3 playerPos, int initialPoolSize)
        {
            List<Vector2> vectorList = new List<Vector2>();
            for (int i = 0; i < initialPoolSize; i++)
            {
                int x = UnityEngine.Random.Range(-30, 30);
                int xV = x == playerPos.x ? 30 : x;
                int y = UnityEngine.Random.Range(-30, 30);
                int yV = y == playerPos.y ? 30 : y;
                vectorList.Add(new Vector2(playerPos.x + xV, playerPos.y + yV));
            }
            for (int i = 0; i < initialPoolSize; i++)
            {
                var obj = _enemyPool.Get();
                obj.gameObject.transform.position = vectorList[i];
                _ActiveEnemyList.Add(obj);
                obj.enemyDead += (enemy) =>
                {
                    EnemyReturnToPool(enemy);
                    obj.enemyDead -= (enemy) => EnemyReturnToPool(enemy);
                };
            }
        }
        public void EnemyReturnToPool(EnemyView enemy)
        {
            EnemyDead?.Invoke(enemy.transform.position);
            _enemyPool.ReturnToPool(enemy);
            _ActiveEnemyList.Remove(enemy);
        }

        public void Dispose()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }

        public void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        public EnemyView GetClosestTarget(Vector3 origin, float range)
        {
            EnemyView closestEnemy = null;
            float closestDistance = Mathf.Infinity;

            foreach (EnemyView enemy in _ActiveEnemyList)
            {
                float distanceToEnemy = Vector3.Distance(enemy.transform.position, origin);

                if (distanceToEnemy <= range)
                {
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = enemy;
                    }
                }
            }

            return closestEnemy;
        }

        public IEnumerable<EnemyView> GetActiveEnemies()
        {
            return _ActiveEnemyList;
        }
    }
}