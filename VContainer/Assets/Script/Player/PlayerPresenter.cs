using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerPresenter : IPostFixedTickable, IStartable
{
    private PlayerView _playerView;
    public ISkillSpeed _skillSpeed;
    public ISkillSheild _skillSheild;
    public ISkillHealth _skillHealth;
    public Action<Vector3> GetClosestEnemyAction;
    private bool IsDamage = true;
    private PlayerData _playerData;

    public PlayerData _dataPlayer
    {
        get => _playerData;
        set
        {
            if (_dataPlayer.Speed != value.Speed)
                SetSpeed();

            _playerData = value;

        }
    }

    public Action PlayerDead;
    public Action<float> PlayerHealtChanged;

    [Inject]
    private void Construct(PlayerView playerView,
        ISkillSpeed speedSkill, ISkillSheild skillSheild, ISkillHealth skillHealth)
    {
        _playerView = playerView;
        _skillSpeed = speedSkill;
        _skillSheild = skillSheild;
        _skillHealth = skillHealth;
    }
    public void Start()
    {
        _playerView.TakeDamage += (enemy) => { OnTakeDamage(enemy); };
        SetSpeed();
    }
    public Vector3 GetPosition() => _playerView.transform.position;

    public Transform GetTransform() => _playerView.transform;

    private void OnTakeDamage(Enemy enemy)
    {
        if (!_dataPlayer.Sheild)
        {
            _playerData.Health -= enemy.Attack;
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
    public void SetSpeed()
    {
        _playerView.SetSpeed(_dataPlayer.Speed);
    }
    public void SetClosestEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            //var obj = _bulletPool.Get();
            //obj.transform.position = _playerView.transform.position;
            //obj.Target(enemy);
            //obj._attackValue = _playerData.Attack;
        }
    }
}
