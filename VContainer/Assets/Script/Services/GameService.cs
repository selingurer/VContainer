using Assets.Script.Services;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameService : IStartable
{
    private ILevelService _levelService;
    private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<Experience> _experiencePool;
    private Player _player;
    private EnemyService _enemyService;
    private GameUIService _gameUIService;
    private ExperienceService _experienceService;

    [Inject]
    private void Construct(ILevelService service, IObjectResolver resolver, Player player, ObjectPool<Enemy> enemyPool, ObjectPool<Experience> expool, EnemyService enemyService, ExperienceService experienceService, GameUIService gameUIService)
    {
        _levelService = service;
        _player = player;
        _enemyPool = enemyPool;
        _enemyService = enemyService;
        _experienceService = experienceService;
        _gameUIService = gameUIService;
        _experiencePool = expool;
    }

    void IStartable.Start()
    {
        _levelService.SetLevel(1);
        CreateEnemy();
        _enemyService._enemyPool = _enemyPool;
        _experienceService._experiencePool = _experiencePool;
        _enemyService._experienceService = _experienceService;
        _experienceService._gameService = this;
        _player._gameService = this;
    }

    public void CreateEnemy()
    {
        List<Vector2> vectorList = new List<Vector2>();
        for (int i = 0; i < _levelService.GetInitialPoolSize(); i++)
        {
            int x = Random.Range(-40, 40);
            int xV = x == _player.transform.position.x ? 30 : x;
            int y = Random.Range(-40, 40);
            int yV = y == _player.transform.position.y ? 30 : y;
            vectorList.Add(new Vector2(_player.transform.position.x + xV, _player.transform.position.y + yV));
        }
        for (int i = 0; i < _levelService.GetInitialPoolSize(); i++)
        {
            var obj = _enemyPool.Get();
            obj.gameObject.transform.position = vectorList[i];
            _enemyService._ActiveEnemyList.Add(obj);
        }
    }
    public void ExperienceValueChanged(int exValue)
    {
        int playerExperienceValue = _player.GetExperienceValue();
        _player.SetExperienceValue(playerExperienceValue += exValue);
        _gameUIService.ExperienceValueChanged(_player.GetExperienceValue(), _levelService.GetExperienceTargetValue());
        if (_player.GetExperienceValue() >= _levelService.GetExperienceTargetValue())
        {
            _player.SetExperienceValue(0);
            _levelService.SetLevel(_levelService.GetLevel() + 1);
            _gameUIService.ExperienceValueChanged(0, _levelService.GetExperienceTargetValue());
            _ = _gameUIService.LevelChanged(_levelService.GetLevel());
            CreateEnemy();
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
}

