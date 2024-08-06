using UnityEngine;
using VContainer;

public class SkillData
{
    public Sprite SpriteSkill;
    public string DescSkill;
}
public class Skill : ISkill
{
    public ISkillService _skillService;
    public SkillTypes _skillTypes;
    public Player _player;
    private SkillData _data;
    private GameUIPanel _gameUIPanel;
    public SkillData Data
    {
        get => _data;
        set
        {
            _data = value;
            _skillService.AddSkill(this);
        }
    }

    public SkillTypes SkillTypes => _skillTypes;

    [Inject]
    private void Construct(ISkillService skillService, Player player,GameUIPanel gameUIPanel)
    {
        _skillService = skillService;
        _player = player;
        _gameUIPanel = gameUIPanel;
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
