namespace LightFencing.Core.Managers
{
    public static class ServiceLocator
    {
        public static DependencyResolverService Resolver { get; set; }

        public static T ResolveDependency<T>()
        {
            return Resolver.Resolve<T>();
        }
    }
}