
using Assets.Script.Services;
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
    private PlayerView _player;
    private EnemyService _enemyService;
    private int _attack;
    public int Attack => _attack;
    private float _speed;
    public float Speed => _speed;

    private float _healt;
    public float Healt
    {
        get => _healt;
        set
        {
            _healt = value;
            if (_healt <= 0)
            {
                _enemyService.EnemyReturnToPool(this);
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
    private void Construct( PlayerView player, EnemyService service)
    {
        _player = player; 
        _enemyService = service;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        int rnd = Random.Range(0, 3);
        _enemyType = (EnemyType)rnd;
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }
    private void MoveTowardsPlayer()
    {
        // Oyuncu ile d��man aras�ndaki y�n vekt�r�n� hesapla
        Vector2 direction = (_player.transform.position - transform.position).normalized;

        // H�z vekt�r�n� hesapla
        Vector2 velocity = direction * _speed;

        // Rigidbody2D'nin h�z�n� ayarla
        _rigidbody.velocity = velocity;

        // D��man�n bak�� y�n�n� ayarla
        SetLookDirection(direction);
    }
    private void SetLookDirection(Vector2 direction)
    {
        // D��man�n y�n�n� oyuncuya �evir
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
        }
    }
}
