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
        public List<EnemyView> _ActiveEnemyList = new();
        private ObjectPool<EnemyView> _enemyPool;
        private CancellationTokenSource cancellationTokenSource;
        public Action<Vector3> EnemyDead;


        [Inject]
        private void Construct(ObjectPool<EnemyView> objectPoolEnemy)
        {
            _enemyPool = objectPoolEnemy;
        }
        public void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
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
                int rnd = UnityEngine.Random.Range(0, 3);
                EnemyTypeSet(obj, (EnemyType)rnd);
                obj.gameObject.transform.position = vectorList[i];
                _ActiveEnemyList.Add(obj);
                obj._isEnemyActivated = true;
                obj.enemyDead += (enemy) =>
                {
                    EnemyReturnToPool(enemy);
                    obj.enemyDead -= (enemy) => EnemyReturnToPool(enemy);
                };
            }
        }
        private void EnemyTypeSet(EnemyView obj, EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Attack:
                    EnemyData dataAttack = new EnemyData()
                    {
                        Attack = 20,
                        Health = 100,
                        Speed = 2f,
                        EnemyType = type,
                    };
                    obj._enemyData = dataAttack;
                    obj.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case EnemyType.Heart:
                    EnemyData dataHeart = new EnemyData()
                    {
                        Attack = 15,
                        Health = 150,
                        Speed = 2f,
                        EnemyType = type,
                    };
                    obj._enemyData = dataHeart;
                    obj.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case EnemyType.Speed:
                    EnemyData dataSpeed = new EnemyData()
                    {
                        Attack = 10,
                        Health = 100,
                        Speed = 3f,
                        EnemyType = type,
                    };
                    obj._enemyData = dataSpeed;
                    obj.StartShooting();
                    obj.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
            }
        }
        public void EnemyReturnToPool(EnemyView enemy)
        {
            EnemyDead?.Invoke(enemy.transform.position);
            _enemyPool.ReturnToPool(enemy);
            _ActiveEnemyList.Remove(enemy);
        }
        
        public IEnumerable<EnemyView> GetActiveEnemies()
        {
            return _ActiveEnemyList;
        }
    }
}