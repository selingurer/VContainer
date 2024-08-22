using Assets.Script.Services;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerService : IPostFixedTickable, IStartable 
{
    private PlayerView _playerView;
    public ISkillSpeed _skillSpeed;
    public ISkillShield _skillSheild;
    public ISkillHealth _skillHealth;
    private IClosestTargetLocator<EnemyView> _closestTargetLocator;
    private bool IsDamage = true;
    private PlayerData _playerData;
    private IBulletSpawnerService _bulletSpawnerService;
    private IEnemyService _enemyService;
    public PlayerData _dataPlayer
    {
        get => _playerData;
        set
        {
            _playerData = value;
        }
    }

    public Action PlayerDead;
    public Action<float> PlayerHealtChanged;

    [Inject]
    private void Construct(PlayerView playerView, ISkillSpeed speedSkill, ISkillShield skillSheild, ISkillHealth skillHealth,
        IBulletSpawnerService bulletSpawnerService, PlayerData dataPlayer, IClosestTargetLocator<EnemyView> closeTargetLocator,
        IEnemyService enemyService)
    {
        _playerView = playerView;
        _skillSpeed = speedSkill;
        _skillSheild = skillSheild;
        _skillHealth = skillHealth;
        _bulletSpawnerService = bulletSpawnerService;
        _dataPlayer = dataPlayer;
        _closestTargetLocator = closeTargetLocator;
        _enemyService = enemyService;
    }
    public void Start()
    {
        _playerView.TakeDamage += (attackValue) => { OnTakeDamage(attackValue); };
    }
    public Vector3 GetPosition() => _playerView.transform.position;

    public Transform GetTransform() => _playerView.transform;

    private void OnTakeDamage(float attackValue)
    {
        if (!_dataPlayer.Shield)
        {
            _playerData.Health -= attackValue;
            if (_playerData.Health < 0)
                PlayerDead?.Invoke();
            PlayerHealtChanged?.Invoke(_dataPlayer.Health);
        }
    }

    public void PostFixedTick()
    {
        GetEnemyClosestActionSetAsync();
    }
    private async void GetEnemyClosestActionSetAsync()
    {
        if (!IsDamage)
            return;

        var activeEnemies = _enemyService.GetActiveEnemies();
        var closestEnemy = _closestTargetLocator.GetClosestTarget(
             _playerView.transform.position,
             activeEnemies,
             enemy => enemy.transform.position,
             5f
         );
        if (closestEnemy != null)
            SetClosestEnemy(closestEnemy);

        IsDamage = false;
        await UniTask.Delay(1000);
        IsDamage = true;
    }
    public void SetClosestEnemy(EnemyView enemy)
    {
        if (enemy != null)
        {
            _bulletSpawnerService.GetBullet(enemy, _playerView, _dataPlayer.Attack);
        }
    }
}
