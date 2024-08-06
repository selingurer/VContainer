
using UnityEngine;

public class LevelService : ILevelService
{
    private int _level;
    private int initialEnemyPoolSize = 15;
    public int _experienceTargetValue = 50;
    
    public void SetLevel(int level)
    {
        if (_level > level)
        {
            LevelIncrease();
        }
        _level = level;
    }

    public int GetLevel()
    {
        return _level;
    }

    public void LevelIncrease()
    {
        initialEnemyPoolSize += initialEnemyPoolSize / 20;
        _experienceTargetValue += (int)(_experienceTargetValue * 1.5f);
    }

    public int GetInitialPoolSize()
    {
        return initialEnemyPoolSize;
    }

    public int GetExperienceTargetValue()
    {
        return _experienceTargetValue;
    }
}
