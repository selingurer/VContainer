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

public class EnemyView : MonoBehaviour, ITargetable<EnemyView>, IDisposable
{
    private Rigidbody2D _rigidbody;
    private ITargetable<PlayerView> _target;
    public EnemyData _enemyData;
    public Action<EnemyView> enemyDead;
    private IBulletSpawnerService _bulletSpawnerService;
    private IClosestTargetLocator<PlayerView> _closestTargetLocator;
    private CancellationTokenSource _cancellationTokenSource;
    public bool _isEnemyActivated = true;

    [Inject]
    private void Construct(ITargetable<PlayerView> target,IBulletSpawnerService bulletService, IClosestTargetLocator<PlayerView> closeTargetLocator)
    {
        _target = target;
        _bulletSpawnerService = bulletService;
        _closestTargetLocator = closeTargetLocator;
    }
    public void StartShooting()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        ShootAtTargetPeriodically(_cancellationTokenSource.Token).Forget();
    }
    private async UniTaskVoid ShootAtTargetPeriodically(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await UniTask.Delay(1000, cancellationToken: cancellationToken);

                if (_target != null && this != null && _bulletSpawnerService != null)
                {
                    ShootAtTarget();
                }
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
                  _target.GetTargetPos(),
                  _target.GetTarget(), this.transform.position, 4.5f
              );
        var targetAsComponent = _target as ITargetable<Component>;

        if (closestTarget != null)
            _bulletSpawnerService.GetBullet(targetAsComponent, this, _enemyData.Attack);
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
        Vector2 direction = (_target.GetTargetPos() - transform.position).normalized;
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

    public Vector3 GetTargetPos()
    {
        return transform.position;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = null;

    }
    EnemyView ITargetable<EnemyView>.GetTarget()
    {
        return this;
    }
}
