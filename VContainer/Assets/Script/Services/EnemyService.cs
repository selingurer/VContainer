using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Script.Services
{
    public class EnemyService : IStartable
    {
        public ExperienceService _experienceService;
        public List<Enemy> _ActiveEnemyList = new List<Enemy>();
        public ObjectPool<Enemy> _enemyPool;
        private ILevelService _levelService;
        public PlayerView _player;

        [Inject]
        private void Construct(ILevelService levelServis)
        {
            _levelService = levelServis;
        }
       
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
        public void CreateEnemy()
        {
            List<Vector2> vectorList = new List<Vector2>();
            for (int i = 0; i < _levelService.GetInitialPoolSize(); i++)
            {
                int x = Random.Range(-30, 30);
                int xV = x == _player.transform.position.x ? 30 : x;
                int y = Random.Range(-30,30);
                int yV = y == _player.transform.position.y ? 30 : y;
                vectorList.Add(new Vector2(_player.transform.position.x + xV, _player.transform.position.y + yV));
            }
            for (int i = 0; i < _levelService.GetInitialPoolSize(); i++)
            {
                var obj = _enemyPool.Get();
                obj.gameObject.transform.position = vectorList[i];
                _ActiveEnemyList.Add(obj);
            }
        }
        public void EnemyReturnToPool(Enemy enemy)
        {
            _experienceService.GetExperience(enemy.transform.position);
            _enemyPool.ReturnToPool(enemy);
            _ActiveEnemyList.Remove(enemy);
            if(_ActiveEnemyList.Count < 10)
            {
                CreateEnemy();
            }
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

    }
}