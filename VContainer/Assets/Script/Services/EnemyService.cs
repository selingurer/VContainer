
using Cysharp.Threading.Tasks;
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

        public void EnemyReturnToPool(Enemy enemy)
        {
            _experienceService.GetExperience(enemy.transform.position);
            _enemyPool.ReturnToPool(enemy);
            _ActiveEnemyList.Remove(enemy);
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }
    }
}