using Assets.Script.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerPresenter : IFixedTickable, IStartable
{
    private PlayerView _playerView;
    private PlayerService _playerService;
    private PlayerData _dataPlayer;
    public bool _shield = false;

    [Inject]
    private void Construct(PlayerService playerService, PlayerView playerView, ObjectPool<Bullet> bulletPool, EnemyService enemyService)
    {
        _playerView = playerView;
        _playerService = playerService;
        _playerService.SetService(bulletPool, enemyService);
        _playerService.SetAttack(_dataPlayer.Attack);
    }
    public void Tick()
    {
        throw new System.NotImplementedException();
    }

    public void FixedTick()
    {
       _playerService.Shoot(_playerView.transform.position).Forget();
    }

    public void Start()
    {
        _playerView.TakeDamage += (enemy) => { OnTakeDamage(enemy); };
    }
    private void OnTakeDamage(Enemy enemy)
    {
        if (!_shield)
            _playerService.Damage(enemy.Attack);
    }
}
