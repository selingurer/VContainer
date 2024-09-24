using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerService : IPostFixedTickable, IStartable, IDisposable,ISkillShield,ISkillHealth,ISkillSpeed
{
    private PlayerView _playerView;
    private ClosestTargetLocator<EnemyView> _closestTargetLocator;
    private bool IsDamage = true;
    private PlayerData _playerData;
    private IBulletSpawnerService _bulletSpawnerService;
    private IGetActiveComponentList _enemyService;
    private CancellationTokenSource _cancellationTokenSource;
    private IObjectResolver _objectResolver;
    private GameObject _skillSheildObject; 
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
    private void Construct(PlayerView playerView, ISkillSpeed speedSkill,
        IBulletSpawnerService bulletSpawnerService, PlayerData dataPlayer, ClosestTargetLocator<EnemyView> closeTargetLocator,
        IGetActiveComponentList enemyService, IObjectResolver objectResolver, GameObject skillSheildObject)
    {
        _playerView = playerView;
        _bulletSpawnerService = bulletSpawnerService;
        _dataPlayer = dataPlayer;
        _closestTargetLocator = closeTargetLocator;
        _enemyService = enemyService;
        _objectResolver = objectResolver;
        _skillSheildObject = skillSheildObject;
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

    public async UniTask SetSkillShield()
    {
        try
        {
            _dataPlayer.Shield = true;
            var objectSkill = _objectResolver.Instantiate(_skillSheildObject);
            objectSkill.transform.SetParent(_playerView.transform, false);
            
            await UniTask.Delay(10000, cancellationToken: _cancellationTokenSource.Token);
            
            if (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                objectSkill.gameObject.SetActive(false);
                _dataPlayer.Shield = false;
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Skill shield task was cancelled.");
        }
        catch (MissingReferenceException ex)
        {
            Debug.LogError("Missing reference: " + ex.Message);
        }
    }

    public void SetSkillHealth()
    {
        _dataPlayer.Health = _dataPlayer.FirstHealth;
    }

    public float SpeedBost { get => 1.5f; }

    public void SetSkillSpeed()
    {
        _dataPlayer.Speed *= SpeedBost; 
    }
}
