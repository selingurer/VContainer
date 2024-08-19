using System;
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
    private Rigidbody2D _rigidbody;
    private PlayerView _player;
    [Inject] private EnemyData _enemyData;
    public Action<EnemyView> enemyDead;

    private EnemyType _enemyType
    {
        set
        {
            switch (value)
            {
                case EnemyType.Attack:
                    _enemyData.Attack = 20;
                    _enemyData.Health = 100;
                    _enemyData.Speed = 2f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case EnemyType.Heart:
                    _enemyData.Health = 110;
                    _enemyData.Attack = 10;
                    _enemyData.Speed = 2f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case EnemyType.Speed:
                    _enemyData.Health = 100;
                    _enemyData.Attack = 10;
                    _enemyData.Speed = 3f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
            }
        }
    }
    [Inject]
    private void Construct(PlayerView player)
    {
        _player = player;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        int rnd = UnityEngine.Random.Range(0, 3);
        _enemyType = (EnemyType)rnd;
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }
    private void MoveTowardsPlayer()
    {
        Vector2 direction = (_player.transform.position - transform.position).normalized;
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
        if(_enemyData.Health <= 0)
        {
            enemyDead?.Invoke(this);
        }
    }

    public float GetAttackValue()
    {
       return _enemyData.Attack;
    }
}
