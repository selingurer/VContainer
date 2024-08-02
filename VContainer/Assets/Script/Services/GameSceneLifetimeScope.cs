using Assets.Script.Services;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

public class GameSceneLifetimeScope : LifetimeScope
{
    [SerializeField] private Enemy objEnemyPrefab;
    [SerializeField] private Transform transformEnemy;
    [SerializeField] private Player objPlayer;
    [SerializeField] private Bullet objBullet;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<LevelService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<Speed>(Lifetime.Transient).AsImplementedInterfaces().AsSelf();
        builder.Register<Healt>(Lifetime.Transient).AsImplementedInterfaces();
        builder.Register<Attack>(Lifetime.Transient).AsImplementedInterfaces();
        builder.Register<ObjectPool<Enemy>>(Lifetime.Singleton).WithParameter(objEnemyPrefab).WithParameter(20);
        builder.RegisterInstance(objBullet);
        builder.Register<ObjectPool<Bullet>>(Lifetime.Singleton).WithParameter(objBullet).WithParameter(20);
        builder.RegisterInstance(objPlayer).WithParameter(objBullet);
        builder.Register<Enemy>(Lifetime.Transient).WithParameter(objPlayer);
        builder.Register<EnemyService>(Lifetime.Singleton).AsSelf();
        builder.Register<GameService>(Lifetime.Singleton).AsImplementedInterfaces()
            .WithParameter(objEnemyPrefab)
            .WithParameter(transformEnemy)
            .WithParameter(objPlayer);
    }
}
