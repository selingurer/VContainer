
using UnityEngine;

public class LevelService : ILevelService
{
    private int _level;
    public void SetLevel(int level)
    {
        if (_level > level)
        {
            LevelIncrease();
        }
        _level = level;
        Debug.Log(_level);
    }

    public int GetLevel()
    {
        return _level;
    }

    public void LevelIncrease()
    {

    }
}
