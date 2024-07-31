using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<LevelService>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<Speed>(Lifetime.Singleton).AsSelf();
    }
}
