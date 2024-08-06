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
    [SerializeField] private Experience objExperience;
    [SerializeField] private GameUIPanel gameUIPanel;
    [SerializeField] private SkillUI objSkillUI;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<LevelService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<Speed>(Lifetime.Transient).AsImplementedInterfaces().AsSelf();
        builder.Register<Healt>(Lifetime.Transient).AsImplementedInterfaces();
        builder.Register<Attack>(Lifetime.Transient).AsImplementedInterfaces();
        builder.RegisterInstance(objBullet);
        builder.RegisterInstance(gameUIPanel);
        builder.RegisterInstance(objSkillUI);
        builder.RegisterInstance(objPlayer).WithParameter(objBullet);
        builder.Register<Enemy>(Lifetime.Transient).WithParameter(objPlayer); 

        builder.Register<ObjectPool<Enemy>>(Lifetime.Singleton).WithParameter(objEnemyPrefab).WithParameter(20);
        builder.Register<ObjectPool<Bullet>>(Lifetime.Singleton).WithParameter(objBullet).WithParameter(20);
        builder.Register<ObjectPool<Experience>>(Lifetime.Singleton).WithParameter(objExperience).WithParameter(20);
        builder.Register<Skill>(Lifetime.Transient).AsImplementedInterfaces();
        builder.Register<ExperienceService>(Lifetime.Singleton).AsSelf();
        builder.Register<EnemyService>(Lifetime.Singleton).AsSelf();
        builder.Register<Experience>(Lifetime.Transient);
        builder.Register<SkillService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.Register<GameService>(Lifetime.Singleton).AsImplementedInterfaces()
            .WithParameter(objEnemyPrefab)
            .WithParameter(transformEnemy)
            .WithParameter(objPlayer).WithParameter(gameUIPanel);


        builder.Register<SkillSpeed>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<SkillHeart>(Lifetime.Singleton).AsImplementedInterfaces();
    }
}
