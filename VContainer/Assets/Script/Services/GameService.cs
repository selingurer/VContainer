using Assets.Script.Services;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameService : IStartable
{
    private ILevelService _levelService;
    private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<Experience> _experiencePool;
    private PlayerView _player;
    private EnemyService _enemyService;
    private GameUIPanel _gameUIService;
    private ExperienceService _experienceService;
    private SkillService _skillService;
    [Inject]
    private void Construct(ILevelService levelService, IObjectResolver resolver, PlayerView player, ObjectPool<Enemy> enemyPool,
        ObjectPool<Experience> experiencePool, EnemyService enemyService, ExperienceService experienceService, GameUIPanel gameUIService,SkillService skillService)
    {
        _levelService = levelService;
        _enemyPool = enemyPool;
        _experiencePool = experiencePool;
        _enemyService = enemyService;
        _experienceService = experienceService;
        _gameUIService = gameUIService;
        _skillService = skillService;
        _player = player;
    }

    void IStartable.Start()
    {
        _levelService.SetLevel(1);
        _enemyService._enemyPool = _enemyPool;
        _enemyService.CreateEnemy();
        _experienceService._experiencePool = _experiencePool;
        _enemyService._experienceService = _experienceService;
        _experienceService._gameService = this;
        _player._gameService = this;
        float playerHeartValue = _player.Healt;
        _gameUIService.SliderHeartValueChanged((int)playerHeartValue, (int)playerHeartValue);
    }

    
    public async void ExperienceValueChanged(int exValue)
    {
        int playerExperienceValue = _player.GetExperienceValue();
        _player.SetExperienceValue(playerExperienceValue += exValue);
        _ = _gameUIService.ExperienceValueChanged(_player.GetExperienceValue(), _levelService.GetExperienceTargetValue());
        if (_player.GetExperienceValue() >= _levelService.GetExperienceTargetValue())
        {
            _player.SetExperienceValue(0);
            _levelService.SetLevel(_levelService.GetLevel() + 1);
            await _gameUIService.ExperienceValueChanged(0, _levelService.GetExperienceTargetValue());
            _enemyService.CreateEnemy();
            await _gameUIService.LevelChanged(_levelService.GetLevel());
            _gameUIService.CreateSkill(_skillService.GetSkillList());
        }
    }
    public void GameOver()
    {
        GameOverData data = new GameOverData()
        {
            totalExperience = _player.GetTotalExperienceValue(),
            level = _levelService.GetLevel(),
        };
        _gameUIService.GameOver(data);

    }
    public async UniTask PlayerHeartChanged()
    {
        await _gameUIService.SliderHeartValueChanged((int)_player.Healt, (int)_player.FirstHealt);
    }
}

