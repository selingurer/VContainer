using System;
using UnityEngine;
using VContainer;
public enum EnemyType
{
    Attack = 0,
    Heart = 1,
    Speed = 2,
}

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private PlayerPresenter _player;
    private int _attack;
    public int Attack => _attack;
    private float _speed;
    public float Speed => _speed;
    public Action<Enemy> enemyDead;
    private float _healt;
    public float Healt
    {
        get => _healt;
        set
        {
            _healt = value;
            if (_healt <= 0)
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
                    _attack = 20;
                    _healt = 100;
                    this._speed = 2f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case EnemyType.Heart:
                    _healt = 110;
                    _attack = 10;
                    this._speed = 2f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case EnemyType.Speed:
                    _healt = 100;
                    _attack = 10;
                    this._speed = 3f;
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
            }
        }
    }
    [Inject]
    private void Construct( PlayerPresenter player  )
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
}
