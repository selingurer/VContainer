
public class LevelController : ILevelSytem
{
    private int _level;
    public void SetLevel(int level)
    {
        _level = level;
    }

    int ILevelSytem.GetLevel()
    {
        return _level;
    }
}
