using Assets.Script.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameSceneLifetimeScope : LifetimeScope
{
    [SerializeField] private EnemyView objEnemyPrefab;
    [SerializeField] private Transform transformEnemy;
    [SerializeField] private PlayerView objPlayer;
    [SerializeField] private BulletView objBullet;
    [SerializeField] private ExperienceView objExperience;
    [SerializeField] private GameUIPanel gameUIPanel;
    [SerializeField] private SkillCardUI objSkillCardUI;
    [SerializeField] private GameObject objSheildSkill;
    [SerializeField] private SkillData skillData;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<LevelService>(Lifetime.Singleton).AsSelf();
        builder.RegisterInstance(objBullet);
        builder.RegisterInstance(gameUIPanel);
        builder.RegisterInstance(objSkillCardUI);
        builder.RegisterInstance(objPlayer).WithParameter(objBullet);
        builder.Register<EnemyView>(Lifetime.Transient).WithParameter(objPlayer);

        //Skill
        builder.Register<SkillHealth>(Lifetime.Transient).AsImplementedInterfaces();
        builder.Register<SkillSpeed>(Lifetime.Transient).AsImplementedInterfaces();
        builder.Register<SkillShield>(Lifetime.Transient).AsImplementedInterfaces().WithParameter(objSheildSkill);

        //Player
        builder.Register<PlayerService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(new PlayerData());

        builder.Register<ObjectPool<EnemyView>>(Lifetime.Singleton).WithParameter(objEnemyPrefab).WithParameter(20);
        builder.Register<ObjectPool<BulletView>>(Lifetime.Singleton).WithParameter(objBullet).WithParameter(20);
        builder.Register<ObjectPool<ExperienceView>>(Lifetime.Singleton).WithParameter(objExperience).WithParameter(20);
        builder.Register<BulletSpawnerService>(Lifetime.Singleton).AsSelf();
        builder.Register<ExperienceService>(Lifetime.Singleton).AsSelf();
        builder.Register<EnemyService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.Register<ExperienceView>(Lifetime.Transient);
        builder.Register<SkillService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(skillData);
        builder.Register<GameService>(Lifetime.Singleton).AsImplementedInterfaces()
            .WithParameter(objEnemyPrefab)
            .WithParameter(transformEnemy)
            .WithParameter(objPlayer).WithParameter(gameUIPanel);
    }
}
