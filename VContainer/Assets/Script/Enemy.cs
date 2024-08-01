
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

public class Enemy : MonoBehaviour
{
    public ISpeed _speed;
    public IHealt _healt;
    public IAttack _attack;
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
                    this._speed.SetSpeed(0.4f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case EnemyType.Heart:
                    _healt.SetHealt(110);
                    _attack.SetAttack(10);
                    this._speed.SetSpeed(0.4f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case EnemyType.Speed:
                    _healt.SetHealt(100);
                    _attack.SetAttack(10);
                    this._speed.SetSpeed(0.5f);
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
            }
        }
    }
    [Inject]
    private void Construct(ISpeed speed, IHealt healt, IAttack attack,Player player)
    {
        _speed = speed;
        _healt = healt;
        _attack = attack;
        _player = player;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        int rnd = Random.Range(0, 3);
        _enemyType = (EnemyType)rnd;
    }
    private void Update()
    {
        _rigidbody.DOMove((Vector2)_player.transform.position, _speed.GetSpeed());
        Debug.Log(_speed.GetSpeed());
    }

}
