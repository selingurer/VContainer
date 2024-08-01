using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameSceneLifetimeScope : LifetimeScope
{
    [SerializeField] public Enemy objEnemyPrefab;
    [SerializeField] public Transform transformEnemy;
    [SerializeField] public Player objPlayer;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<LevelService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<Speed>(Lifetime.Transient).AsImplementedInterfaces().AsSelf();
        builder.Register<Healt>(Lifetime.Transient).AsImplementedInterfaces();
        builder.Register<Attack>(Lifetime.Transient).AsImplementedInterfaces();
        builder.Register<ObjectPool<Enemy>>(Lifetime.Singleton)
              .WithParameter(objEnemyPrefab)
              .WithParameter(Container) // Container (IObjectResolver) çözümleyici
              .WithParameter(transformEnemy); // Parent transform
        builder.RegisterInstance(objPlayer);
        builder.RegisterInstance(objEnemyPrefab).WithParameter(objPlayer);
        builder.Register<GameService>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(objEnemyPrefab).WithParameter(transformEnemy);
    }
}
