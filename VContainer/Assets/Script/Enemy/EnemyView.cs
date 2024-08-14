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
    private PlayerService _player;
   
    public int Attack { get; set; }
    private float _speed;
    public Action<EnemyView> enemyDead;
    public float Health
    {
        get => Health;
        set
        {
            if (value <= 0)
            {
                enemyDead?.Invoke(this);
            }
        }
    }
    private EnemyType _enemyType
    {
        set
        {
            switch (value)
            {
                case EnemyType.Attack:
                    Attack = 20;
                    Health = 100;
                    this._speed = 2f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case EnemyType.Heart:
                    Health = 110;
                    Attack = 10;
                    this._speed = 2f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case EnemyType.Speed:
                    Health = 100;
                    Attack = 10;
                    this._speed = 3f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
            }
        }
    }
    [Inject]
    private void Construct(PlayerService player)
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
        Vector2 direction = (_player.GetPosition() - transform.position).normalized;
        Vector2 velocity = direction * _speed;
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
        Health -= damage;
    }
}
