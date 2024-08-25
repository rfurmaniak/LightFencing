using Zenject;

namespace LightFencing.Core.Managers
{
    public class DependencyResolverService
    {
        private readonly DiContainer _container;

        public DependencyResolverService(DiContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}