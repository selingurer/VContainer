using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerService : IPostFixedTickable, IStartable
{
    private PlayerView _playerView;
    public ISkillSpeed _skillSpeed;
    public ISkillShield _skillSheild;
    public ISkillHealth _skillHealth;
    public Action<Vector3> GetClosestEnemyAction;
    private bool IsDamage = true;
    private PlayerData _playerData;
    private BulletSpawnerService _bulletSpawnerService;

    public PlayerData _dataPlayer
    {
        get => _playerData;
        set
        {
            if (_dataPlayer is not null && _dataPlayer.Speed != value.Speed)
                SetSpeed();

            _playerData = value;

        }
    }

    public Action PlayerDead;
    public Action<float> PlayerHealtChanged;

    [Inject]
    private void Construct(PlayerView playerView,
        ISkillSpeed speedSkill, ISkillShield skillSheild, ISkillHealth skillHealth,BulletSpawnerService bulletSpawnerService,PlayerData dataPlayer)
    {
        _playerView = playerView;
        _skillSpeed = speedSkill;
        _skillSheild = skillSheild;
        _skillHealth = skillHealth;
        _bulletSpawnerService = bulletSpawnerService;
        _dataPlayer = dataPlayer;
    }
    public void Start()
    {
        _playerView.TakeDamage += (attackValue) => { OnTakeDamage(attackValue); };
        SetSpeed();
    }
    public Vector3 GetPosition() => _playerView.transform.position;

    public Transform GetTransform() => _playerView.transform;

    private void OnTakeDamage(float attackValue)
    {
        if (!_dataPlayer.Shield)
        {
            _playerData.Health -= attackValue;
            if (_playerData.Health < 0)
                PlayerDead?.Invoke();
            PlayerHealtChanged?.Invoke(_dataPlayer.Health);
        }
    }

    public void PostFixedTick()
    {
        GetEnemyClosestActionSetAsync().Forget();
    }
    private async UniTask GetEnemyClosestActionSetAsync()
    {
        if (!IsDamage)
            return;

        GetClosestEnemyAction?.Invoke(_playerView.transform.position);
        IsDamage = false;
        await UniTask.Delay(1000);
        IsDamage = true;
    }
    private void SetSpeed()
    {
        _playerView.SetSpeed(_dataPlayer.Speed);
    }
    public void SetClosestEnemy(EnemyView enemy)
    {
        if (enemy != null)
        {
            _bulletSpawnerService.GetBullet(enemy,_playerView,_playerView.transform.position,_dataPlayer.Attack);
        }
    }
}
