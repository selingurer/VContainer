
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
public enum EnemyType
{
    Attack = 0,
    Heart = 1,
    Speed = 2,
}

public class Enemy : BaseCharacter
{
    private Rigidbody2D _rigidbody;
    private Player _player;
    private EnemyType _enemyType
    {
        set
        {
            switch (value)
            {
                case EnemyType.Attack:
                    _attack.SetAttack(20);
                    _healt.SetHealt(100);
                    this._speed.SetSpeed(3f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case EnemyType.Heart:
                    _healt.SetHealt(110);
                    _attack.SetAttack(10);
                    this._speed.SetSpeed(3f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case EnemyType.Speed:
                    _healt.SetHealt(100);
                    _attack.SetAttack(10);
                    this._speed.SetSpeed(4f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
            }
        }
    }
    [Inject]
    private void Construct(ISpeed speed, IHealt healt, IAttack attack, Player player)
    {
        _speed = speed;
        _healt = healt;
        _attack = attack;
        _player = player;
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
        Debug.Log(_speed.GetSpeed());
    }
    private void MoveTowardsPlayer()
    {
        // Oyuncu ile d��man aras�ndaki y�n vekt�r�n� hesapla
        Vector2 direction = (_player.transform.position - transform.position).normalized;

        // H�z vekt�r�n� hesapla
        Vector2 velocity = direction * _speed.GetSpeed();

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
