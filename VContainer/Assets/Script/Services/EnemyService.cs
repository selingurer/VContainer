
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.Script.Services
{
    public class EnemyService
    {
        public List<Enemy> _ActiveEnemyList = new List<Enemy>();

        public Enemy GetClosestEnemy(Vector3 origin)
        {

            foreach (Enemy enemy in _ActiveEnemyList)
            {

                if (Vector3.Distance(enemy.transform.position, origin) < 4)
                {
                    return enemy;
                }
            }
            return null;
        }
    }
}