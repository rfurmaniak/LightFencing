using UnityEngine;
using Zenject;

namespace LightFencing.Core.Managers
{
    public class ServiceLocatorInitializer : MonoBehaviour
    {
        [Inject]
        private void Construct(DependencyResolverService dependencyResolver)
        {
            ServiceLocator.Resolver = dependencyResolver;
        }
    }
}