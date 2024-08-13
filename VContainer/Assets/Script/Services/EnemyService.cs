using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Script.Services
{
    public class EnemyService : IDisposable, IStartable
    {
        public List<Enemy> _ActiveEnemyList = new List<Enemy>();
        [Inject] private ObjectPool<Enemy> _enemyPool;
        private CancellationTokenSource cancellationTokenSource;
        public Action<Vector3> EnemyDead;

      
        public Enemy GetClosestEnemy(Vector3 origin)
        {

            foreach (Enemy enemy in _ActiveEnemyList)
            {

                if (Vector3.Distance(enemy.transform.position, origin) < 5 && Vector3.Distance(enemy.transform.position, origin) > 2)
                {
                    return enemy;
                }
            }
            return null;
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
        public void EnemyReturnToPool(Enemy enemy)
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
    }
}