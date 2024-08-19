using Assets.Script.Services;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public static class GameEvents
{
    public static Action<float, float> OnPlayerHealthChanged;
    public static Action<int, int> OnExperienceChanged;
    public static Action<int,List<SkillData>> OnLevelUp;
    public static Action<GameOverData> OnGameOver;
    public static Action OnSkillSelected;
}
public class GameService : IStartable, IDisposable
{
    private LevelService _levelService;
    private PlayerService _playerService;
    private EnemyService _enemyService;
    private ExperienceService _experienceService;
    private SkillService _skillService;
    [Inject]
    private void Construct(LevelService levelService, PlayerService player,
         EnemyService enemyService, ExperienceService experienceService , SkillService skillService)
    {
        _levelService = levelService;
        _enemyService = enemyService;
        _experienceService = experienceService;
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
        GameEvents.OnExperienceChanged?.Invoke(_playerService._dataPlayer.ExperienceValue, _levelService.GetExperienceTargetValue());
    }

    private void OnSelectSkillCard()
    {
        GameEvents.OnSkillSelected?.Invoke();
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
        GameEvents.OnExperienceChanged?.Invoke(_playerService._dataPlayer.ExperienceValue, _levelService.GetExperienceTargetValue());
        if (_playerService._dataPlayer.ExperienceValue >= _levelService.GetExperienceTargetValue())
        {
            _playerService._dataPlayer.ExperienceValue = 0;
            _levelService.SetLevel(_levelService.GetLevel() + 1);
            GameEvents.OnLevelUp?.Invoke(_levelService.GetLevel(), _skillService.GetSkillList());
            GameEvents.OnExperienceChanged?.Invoke(_playerService._dataPlayer.ExperienceValue, _levelService.GetExperienceTargetValue());
            _enemyService.CreateEnemyAsync(_playerService.GetPosition(), _levelService.GetInitialPoolSize()).Forget();
          
        }
    }
    public void OnPlayerDead()
    {
        GameOverData data = new GameOverData()
        {
            totalExperience = _playerService._dataPlayer.TotalExperienceValue,
            level = _levelService.GetLevel(),
        };
        GameEvents.OnGameOver?.Invoke(data);
        ResetToData();


    }
    private void ResetToData()
    {
        _playerService._dataPlayer.ResetToData();
        _playerService._dataPlayer.ResetToPlayerData();
    }
    public async UniTask OnPlayerHealthChanged(float heartValue)
    {
        GameEvents.OnPlayerHealthChanged?.Invoke(heartValue, _playerService._dataPlayer.FirstHealth);
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

