using System.Collections.Generic;
using UnityEngine;

public interface IEnemyService
{
    IEnumerable<EnemyView> GetActiveEnemies();
}