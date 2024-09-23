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

public interface IEnemyData
{
    public Vector3 GetPosition();
    public ITargetable GetTargetable();
}

public class EnemyView : MonoBehaviour, ITargetable, IEnemyData
{
    private Rigidbody2D _rigidbody;
    public EnemyData _enemyData;
    public Action<EnemyView> enemyDead;
    private IBulletSpawnerService _bulletSpawnerService;
    private IClosestTargetLocator<PlayerView> _closestTargetLocator;
    private CancellationTokenSource _cancellationTokenSource = new();
    public bool _isEnemyActivated = true;
    private IPlayerData _playerData;

    [Inject]
    private void Construct(IBulletSpawnerService bulletService, IClosestTargetLocator<PlayerView> closeTargetLocator,
        IPlayerData playerData)
    {
        _bulletSpawnerService = bulletService;
        _closestTargetLocator = closeTargetLocator;
        _playerData = playerData;
    }

    public void StartShooting()
    {
        //_cancellationTokenSource = new CancellationTokenSource();
        ShootAtTargetPeriodically(_cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid ShootAtTargetPeriodically(CancellationToken cancellationToken)
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

    private void OnDisable()
    {
        _cancellationTokenSource.Cancel();
    }

    public Vector3 GetPosition()
    {
        throw new NotImplementedException();
    }

    public ITargetable GetTargetable()
    {
        return this;
    }
}