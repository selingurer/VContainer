
using Assets.Script.Services;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

public class Player : BaseCharacter
{
    [SerializeField] private FixedJoystick _joystick;
    private Rigidbody2D _rigidbody;
    float _horizontal;
    float _vertical;
    private ObjectPool<Bullet> _bulletPool;
    private EnemyService _enemyService;
    private bool _canDamage = true;
    private readonly int _cooldownTime = 1000;

    [Inject]
    private void Construct(ISpeed speed, IHealt healt, IAttack attack, ObjectPool<Bullet> bulletPool, EnemyService enemyService)
    {
        _speed = speed;
        _healt = healt;
        _attack = attack;
        _bulletPool = bulletPool;
        _enemyService = enemyService;
    }
    private void Awake()
    {
        _speed.SetSpeed(5f);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GetMovementInput();
        SetMovement();
        ToDamage().Forget();
    }
    private void SetMovement()
    {
        _rigidbody.velocity = new Vector2( _horizontal, _vertical) * _speed.GetSpeed();
    }
    private void GetMovementInput()
    {
        _horizontal = _joystick.Horizontal;
        _vertical = _joystick.Vertical;
    }
    private async UniTaskVoid ToDamage()
    {
        if (!_canDamage) return;
        Enemy enemy = _enemyService.GetClosestEnemy(this.transform.position);
        if (enemy != null)
        {
            _canDamage = false; // Ate� edilebilirli�i kapat

            var obj = _bulletPool.Get();
            obj.transform.position = transform.position;
            obj.Target(enemy);

            await UniTask.Delay(_cooldownTime); // Cooldown s�resi kadar bekle

            _canDamage = true; // Cooldown bitti, tekrar ate� edilebilir
        }
    }
}
