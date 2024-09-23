using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerService : IPostFixedTickable, IStartable, IDisposable
{
    public ISkillSpeed _skillSpeed;
    public ISkillShield _skillSheild;
    public ISkillHealth _skillHealth;
    private PlayerView _playerView;
    private ClosestTargetLocator<EnemyView> _closestTargetLocator;
    private bool IsDamage = true;
    private PlayerData _playerData;
    private IBulletSpawnerService _bulletSpawnerService;
    private IGetActiveComponentList _enemyService;
    private CancellationTokenSource _cancellationTokenSource;
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
        IBulletSpawnerService bulletSpawnerService, PlayerData dataPlayer, ClosestTargetLocator<EnemyView> closeTargetLocator,
        IGetActiveComponentList enemyService)
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
        if (_cancellationTokenSource == null) 
            _cancellationTokenSource = new CancellationTokenSource();

        _playerView.TakeDamage +=  OnTakeDamage;
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
        if (_cancellationTokenSource == null || _cancellationTokenSource.Token.IsCancellationRequested)
            return;

        GetEnemyClosestActionSetAsync();
    }
    private async void GetEnemyClosestActionSetAsync()
    {
        if (!IsDamage)
            return;

        try
        {
            if (_playerView == null)
                return;
            var activeEnemies = _enemyService.GetActiveList();
            var closestEnemy = _closestTargetLocator.GetClosestTarget(
                _playerView.transform.position,
                activeEnemies,
                enemy => enemy.transform.position,
                5f
            );

            if (closestEnemy != null)
                SetClosestEnemy((ITargetable)closestEnemy);

            IsDamage = false;
            await UniTask.Delay(1000, cancellationToken: _cancellationTokenSource.Token);
            IsDamage = true;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Enemy closest action was canceled.");
        }
    }
    public void SetClosestEnemy(ITargetable enemy)
    {
        if (enemy != null)
        {
            _bulletSpawnerService.GetBullet(enemy, _playerView, _dataPlayer.Attack);
        }
    }

    public void Dispose()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }
}
