
using Assets.Script.Services;
using Cysharp.Threading.Tasks;
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
    private int _experienceValue;
    private int _totalExperienceValue;
    public GameService _gameService;

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
        _attack.SetAttack(100);
        _healt.FirstHealt = 200;
        _healt.OnHealthChanged += (healt, chara) => { OnHealtChanged(healt, chara); };
    }
    private void OnDestroy()
    {
        _healt.OnHealthChanged -= (healt, chara) => { OnHealtChanged(healt, chara); };
    }
    void FixedUpdate()
    {
        GetMovementInput();
        SetMovement();
        ToDamage().Forget();
    }
    private void SetMovement()
    {
        _rigidbody.velocity = new Vector2(_horizontal, _vertical) * _speed.GetSpeed();
    }
    public void SetExperienceValue(int exValue)
    {
        _experienceValue = exValue;
        _totalExperienceValue = exValue == 0 ? _totalExperienceValue : _totalExperienceValue += exValue;
    }
    public int GetExperienceValue()
    {
        return _experienceValue;
    }
    public int GetTotalExperienceValue()
    {
        return _totalExperienceValue;
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
            _canDamage = false; // Ateþ edilebilirliði kapat

            var obj = _bulletPool.Get();
            obj.transform.position = transform.position;
            obj.Target(enemy);
            obj.SetAttack(_attack);
            obj.SetObjectPool(_bulletPool);

            await UniTask.Delay(_cooldownTime); // Cooldown süresi kadar bekle

            _canDamage = true; // Cooldown bitti, tekrar ateþ edilebilir
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            float attack = collision.gameObject.GetComponent<Enemy>()._attack.GetAttack();
            _healt.SetHealt(_healt.GetHealt() - attack, this);
        }
    }
    private async UniTask OnHealtChanged(float newHealt, BaseCharacter character)
    {
        if (character == this)
        {
            await _gameService.PlayerHeartChanged();
            if (newHealt <= 0)
            {
                _gameService.GameOver();
            }
        }
    }
}
