using Assets.Script.Services;
using UnityEngine;
using VContainer;

public class SkillData
{
    public Sprite SpriteSkill;
    public string DescSkill;
}
public class Skill
{
    public ISkillService _skillService;
    public SkillTypes _skillTypes { get; set; }
    public PlayerView _player;
    public EnemyService _enemyService;
    public GameUIPanel _gameUIPanel;
    private SkillData _data;
    public SkillData Data
    {
        get => _data;
        set
        {
            _data = value;
            _skillService.AddSkill(this);
        }
    }

    [Inject]
    private void Construct(ISkillService skillService, PlayerView player,GameUIPanel gameUIPanel,EnemyService enemyService)
    {
        _skillService = skillService;
        _player = player;
        _gameUIPanel = gameUIPanel;
        _enemyService = enemyService;
    }

    public void SelectSkill()
    {
        SetSkill();
        _gameUIPanel.ClearSkill();
        Time.timeScale = 1;
    }
    protected virtual void SetSkill()
    {

    }


}
