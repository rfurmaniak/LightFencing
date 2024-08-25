using Zenject;

namespace LightFencing.Core.Managers
{
    public class DependencyResolverInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DependencyResolverService>().AsSingle().WithArguments(Container);
        }
    }
}