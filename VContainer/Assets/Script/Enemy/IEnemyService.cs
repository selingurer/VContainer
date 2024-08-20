using System.Collections.Generic;

public interface IEnemyService
{
    IEnumerable<EnemyView> GetActiveEnemies();
}