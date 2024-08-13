using Assets.Script.Services;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameService : IStartable, IDisposable
{
    private ILevelService _levelService;
    private PlayerPresenter _playerPresenter;
    private EnemyService _enemyService;
    private GameUIPanel _gameUIService;
    private ExperienceService _experienceService;
    private SkillService _skillService;
    [Inject]
    private void Construct(ILevelService levelService, IObjectResolver resolver, PlayerPresenter player,
         EnemyService enemyService, ExperienceService experienceService, GameUIPanel gameUIService, SkillService skillService)
    {
        _levelService = levelService;
        _enemyService = enemyService;
        _experienceService = experienceService;
        _gameUIService = gameUIService;
        _skillService = skillService;
        _playerPresenter = player;
    }

    void IStartable.Start()
    {
        _levelService.SetLevel(1);
        _enemyService.CreateEnemyAsync(_playerPresenter.GetPosition(),_levelService.GetInitialPoolSize()).Forget();
        _playerPresenter.PlayerHealtChanged += async (heart) => await OnPlayerHealthChanged(heart);
        _playerPresenter.PlayerDead += OnPlayerDead;
        _playerPresenter.GetClosestEnemyAction += (pos) => OnGetClosestEnemy(pos);
        _experienceService.ExperienceValueChanged += (experienceValue) => OnExperienceValueChangedAsync(experienceValue);
        _enemyService.EnemyDead += (enemyPos) => OnEnemyDead(enemyPos);

        _ = _gameUIService.SliderHeartValueChanged((int)_playerPresenter._dataPlayer.Health, (int)_playerPresenter._dataPlayer.FirstHealth);
    }

    private void OnGetClosestEnemy(Vector3 pos)
    {
        _playerPresenter.SetClosestEnemy(_enemyService.GetClosestEnemy(pos));
    }

    private void OnEnemyDead(Vector3 enemyPos)
    {
        _experienceService.GetExperience(enemyPos);
    }

    public async void OnExperienceValueChangedAsync(int exValue)
    {
        _playerPresenter._dataPlayer.ExperienceValue += exValue;
        _playerPresenter._dataPlayer.TotalExperienceValue += exValue;
        _ = _gameUIService.ExperienceValueChanged(_playerPresenter._dataPlayer.ExperienceValue, _levelService.GetExperienceTargetValue());
        if (_playerPresenter._dataPlayer.ExperienceValue >= _levelService.GetExperienceTargetValue())
        {
            _playerPresenter._dataPlayer.ExperienceValue = 0;
            _levelService.SetLevel(_levelService.GetLevel() + 1);
            await _gameUIService.ExperienceValueChanged(0, _levelService.GetExperienceTargetValue());
            _enemyService.CreateEnemyAsync(_playerPresenter.GetPosition(), _levelService.GetInitialPoolSize()).Forget();
            await _gameUIService.LevelChanged(_levelService.GetLevel());
            _gameUIService.CreateSkill(_skillService.GetSkillList());
        }
    }
    public void OnPlayerDead()
    {
        GameOverData data = new GameOverData()
        {
            totalExperience = _playerPresenter._dataPlayer.TotalExperienceValue,
            level = _levelService.GetLevel(),
        };
        _gameUIService.GameOver(data);

    }
    public async UniTask OnPlayerHealthChanged(float heartValue)
    {
        await _gameUIService.SliderHeartValueChanged((int)heartValue, (int)_playerPresenter._dataPlayer.FirstHealth);
    }
    public void Dispose()
    {
        _playerPresenter.PlayerHealtChanged -= (heart) => _ = OnPlayerHealthChanged(heart);
        _playerPresenter.PlayerDead -= OnPlayerDead;
    }
}

