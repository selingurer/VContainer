using Assets.Script.Services;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameService : IStartable, IDisposable
{
    private LevelService _levelService;
    private PlayerService _playerService;
    private EnemyService _enemyService;
    private GameUIPanel _gameUIPanel;
    private ExperienceService _experienceService;
    private SkillService _skillService;
    [Inject]
    private void Construct(LevelService levelService, IObjectResolver resolver, PlayerService player,
         EnemyService enemyService, ExperienceService experienceService, GameUIPanel gameUIService, SkillService skillService)
    {
        _levelService = levelService;
        _enemyService = enemyService;
        _experienceService = experienceService;
        _gameUIPanel = gameUIService;
        _skillService = skillService;
        _playerService = player;
    }

    void IStartable.Start()
    {
        _levelService.SetLevel(1);
        _enemyService.CreateEnemyAsync(_playerService.GetPosition(),_levelService.GetInitialPoolSize()).Forget();
        _playerService.PlayerHealtChanged += async (heart) => await OnPlayerHealthChanged(heart);
        _playerService.PlayerDead += OnPlayerDead;
        _playerService.GetClosestEnemyAction += (pos) => OnGetClosestEnemy(pos);
        _experienceService.ExperienceValueChanged += (experienceValue) => OnExperienceValueChangedAsync(experienceValue);
        _enemyService.EnemyDead += (enemyPos) => OnEnemyDead(enemyPos);
        _skillService.SelectSkillAction += OnSelectSkillCard;
        _gameUIPanel.SliderHeartValueChanged((int)_playerService._dataPlayer.Health, (int)_playerService._dataPlayer.FirstHealth).Forget();
    }

    private void OnSelectSkillCard()
    {
       _gameUIPanel.ClearSkillCard();
    }

    private void OnGetClosestEnemy(Vector3 pos)
    {
        _playerService.SetClosestEnemy(_enemyService.GetClosestEnemy(pos));
    }

    private void OnEnemyDead(Vector3 enemyPos)
    {
        _experienceService.GetExperience(enemyPos);
    }

    public async void OnExperienceValueChangedAsync(int exValue)
    {
        _playerService._dataPlayer.ExperienceValue += exValue;
        _playerService._dataPlayer.TotalExperienceValue += exValue;
        _ = _gameUIPanel.ExperienceValueChanged(_playerService._dataPlayer.ExperienceValue, _levelService.GetExperienceTargetValue());
        if (_playerService._dataPlayer.ExperienceValue >= _levelService.GetExperienceTargetValue())
        {
            _playerService._dataPlayer.ExperienceValue = 0;
            _levelService.SetLevel(_levelService.GetLevel() + 1);
            await _gameUIPanel.ExperienceValueChanged(0, _levelService.GetExperienceTargetValue());
            _enemyService.CreateEnemyAsync(_playerService.GetPosition(), _levelService.GetInitialPoolSize()).Forget();
            await _gameUIPanel.LevelChanged(_levelService.GetLevel());
            _gameUIPanel.CreateSkillCard(_skillService.GetSkillList());
        }
    }
    public void OnPlayerDead()
    {
        GameOverData data = new GameOverData()
        {
            totalExperience = _playerService._dataPlayer.TotalExperienceValue,
            level = _levelService.GetLevel(),
        };
        _gameUIPanel.GameOver(data);

    }
    public async UniTask OnPlayerHealthChanged(float heartValue)
    {
        await _gameUIPanel.SliderHeartValueChanged((int)heartValue, (int)_playerService._dataPlayer.FirstHealth);
    }
    public void Dispose()
    {
        _playerService.PlayerHealtChanged -= async (heart) => await OnPlayerHealthChanged(heart);
        _playerService.PlayerDead -= OnPlayerDead;
        _playerService.GetClosestEnemyAction -= (pos) => OnGetClosestEnemy(pos);
        _experienceService.ExperienceValueChanged -= (experienceValue) => OnExperienceValueChangedAsync(experienceValue);
        _enemyService.EnemyDead -= (enemyPos) => OnEnemyDead(enemyPos);
        _skillService.SelectSkillAction -= OnSelectSkillCard;
    }
}

