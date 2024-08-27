using System.Collections.Generic;
using UnityEngine;

public interface IEnemyData
{
    public Vector3 GetPosition();
    public ITargetable GetTargetable();
}
public interface IEnemyService
{
    IEnumerable<EnemyView> GetActiveEnemies();
}