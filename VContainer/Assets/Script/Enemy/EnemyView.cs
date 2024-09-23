using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

public enum EnemyType
{
    Attack = 0,
    Heart = 1,
    Speed = 2,
}

public class EnemyView : MonoBehaviour, ITargetable
{
    public bool _isEnemyActivated = true;
    public EnemyData _enemyData;
    public Action<EnemyView> enemyDead;
    private Rigidbody2D _rigidbody;
    private IBulletSpawnerService _bulletSpawnerService;
    private ClosestTargetLocator<PlayerView> _closestTargetLocator;
    private CancellationTokenSource _cancellationTokenSource = new();
    private IPlayerData _playerData;

    [Inject]
    private void Construct(IBulletSpawnerService bulletService, ClosestTargetLocator<PlayerView> closeTargetLocator,
        IPlayerData playerData)
    {
        _bulletSpawnerService = bulletService;
        _closestTargetLocator = closeTargetLocator;
        _playerData = playerData;
    }

    public void StartShooting()
    {
        //_cancellationTokenSource = new CancellationTokenSource();
        ShootAtTargetPeriodicallyAsync(_cancellationTokenSource.Token).Forget();
    }
    public void ShootAtTarget()
    {
        if (!_isEnemyActivated)
            return;

        var closestTarget = _closestTargetLocator.GetClosestTarget(
            _playerData.GetPosition(),
            _playerData.GetComponent(), this.transform.position, 4.5f
        );

        if (closestTarget != null)
            _bulletSpawnerService.GetBullet(_playerData.GetTargetable(), this, _enemyData.Attack);
    }
    public void TakeDamage(float damage)
    {
        _enemyData.Health -= damage;
        if (_enemyData.Health <= 0)
        {
            enemyDead?.Invoke(this);
            _isEnemyActivated = false;
        }
    }

    public float GetAttackValue()
    {
        return _enemyData.Attack;
    }
    private async UniTaskVoid ShootAtTargetPeriodicallyAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(1000, cancellationToken: cancellationToken);
                ShootAtTarget();
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Shooting task was canceled.");
        }
    }

  

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (_playerData.GetPosition() - transform.position).normalized;
        Vector2 velocity = direction * _enemyData.Speed;
        _rigidbody.velocity = velocity;
        SetLookDirection(direction);
    }

    private void SetLookDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
        }
    }
    
    private void OnDisable()
    {
        _cancellationTokenSource.Cancel();
    }

}