
using Assets.Script.Services;
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
    private EnemyService _enemyService;
    private EnemyType _enemyType
    {
        set
        {
            switch (value)
            {
                case EnemyType.Attack:
                    _attack.SetAttack(20);
                    _healt.FirstHealt = 100;
                    this._speed.SetSpeed(3f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case EnemyType.Heart:
                    _healt.FirstHealt = 110;
                    _attack.SetAttack(10);
                    this._speed.SetSpeed(3f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case EnemyType.Speed:
                    _healt.FirstHealt = 100;
                    _attack.SetAttack(10);
                    this._speed.SetSpeed(4f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
            }
        }
    }
    [Inject]
    private void Construct(ISpeed speed, IHealt healt, IAttack attack, Player player, EnemyService service)
    {
        _speed = speed;
        _healt = healt;
        _attack = attack;
        _player = player;
        _enemyService = service;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        int rnd = Random.Range(0, 3);
        _enemyType = (EnemyType)rnd;
        _healt.OnHealthChanged += (healt, chara) => { OnHealtChanged(healt, chara); };
    }

    private void OnHealtChanged(float newHealt, BaseCharacter character)
    {
        if (character == this)
        {
            if (newHealt <= 0)
            {
                _enemyService.EnemyReturnToPool(this);
            }
        }

    }
    private void OnDisable()
    {
        _healt.OnHealthChanged -= (healt, chara) => { OnHealtChanged(healt, chara); };
    }
    private void FixedUpdate()
    {
        MoveTowardsPlayer();
    }
    private void MoveTowardsPlayer()
    {
        // Oyuncu ile düþman arasýndaki yön vektörünü hesapla
        Vector2 direction = (_player.transform.position - transform.position).normalized;

        // Hýz vektörünü hesapla
        Vector2 velocity = direction * _speed.GetSpeed();

        // Rigidbody2D'nin hýzýný ayarla
        _rigidbody.velocity = velocity;

        // Düþmanýn bakýþ yönünü ayarla
        SetLookDirection(direction);
    }
    private void SetLookDirection(Vector2 direction)
    {
        // Düþmanýn yönünü oyuncuya çevir
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
        }
    }
}
