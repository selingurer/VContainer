using Assets.Script.Services;
using System.ComponentModel;
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
    [SerializeField] private PlayerData playerData;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private GameData gameData;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<JoystickInput>(Lifetime.Singleton)
        .As<IInputProvider>()
        .WithParameter(joystick);
        builder.Register<LevelService>(Lifetime.Singleton).AsSelf();
        builder.RegisterInstance(objBullet);
        builder.RegisterInstance(gameUIPanel);
        builder.RegisterInstance(objSkillCardUI);
        builder.RegisterInstance(playerData);
        builder.RegisterInstance(objPlayer).WithParameter(playerData).AsImplementedInterfaces();
        builder.Register<EnemyData>(Lifetime.Transient);
        builder.Register<EnemyView>(Lifetime.Transient).WithParameter(ScriptableObject.CreateInstance<EnemyData>());
        

        //Player
        builder.Register<PlayerService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(playerData).WithParameter(objSheildSkill);

        builder.Register<ClosestTargetLocator<EnemyView>>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.Register<ClosestTargetLocator<PlayerView>>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.Register<ObjectPool<EnemyView>>(Lifetime.Singleton).WithParameter(objEnemyPrefab).WithParameter(15).WithParameter(gameData);
        builder.Register<ObjectPool<BulletView>>(Lifetime.Singleton).WithParameter(objBullet).WithParameter(15).WithParameter(gameData);
        builder.Register<ObjectPool<ExperienceView>>(Lifetime.Singleton).WithParameter(objExperience).WithParameter(15).WithParameter(gameData);
        builder.Register<BulletSpawnerService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.Register<ExperienceService>(Lifetime.Singleton).AsSelf();
        builder.Register<EnemyService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.Register<ExperienceView>(Lifetime.Transient);
        builder.Register<SkillService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(skillData);
        builder.Register<UIService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces().WithParameter(gameUIPanel);
        builder.Register<GameService>(Lifetime.Singleton).AsImplementedInterfaces()
            .WithParameter(objEnemyPrefab)
            .WithParameter(objPlayer);
    }
}
