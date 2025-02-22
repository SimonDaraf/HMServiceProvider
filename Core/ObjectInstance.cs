using System.Reflection;

namespace HMServiceProvider.Core
{
    internal abstract class ObjectInstance : IObjectInstance
    {
        protected ConstructorInfo[]? constructorInfos;
        protected Func<ServiceProvider, object>? factoryDelegate;

        protected ObjectInstance(Type type)
        {
            ConstructorInfo[] local = type.GetConstructors();
            // Order in descending order to first attempt the one with most parameters first.
            constructorInfos = local.OrderByDescending(x => x.GetParameters().Length).ToArray();
        }

        protected ObjectInstance(Func<ServiceProvider, object> factoryDelegate)
        {
            this.factoryDelegate = factoryDelegate;
        }

        public abstract bool TryGetInstance(ServiceProvider serviceProvider, out object? instance);
    }
}
