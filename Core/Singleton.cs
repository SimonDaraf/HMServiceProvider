using System.Reflection;

namespace HMServiceProvider.Core
{
    /// <summary>
    /// Represents a singleton instance.
    /// </summary>
    internal class Singleton : ObjectInstance
    {
        private object? instance;

        public Singleton(Type type) : base(type) { }

        public Singleton(Func<ServiceProvider, object> factoryDelegate) : base(factoryDelegate) { }

        public override bool TryGetInstance(ServiceProvider serviceProvider, out object? instance)
        {
            instance = null;

            // Determine method to pass instance.
            if (this.instance is not null)
            {
                instance = this.instance;
                return true;
            }
            else if (constructorInfos is not null)
            {
                foreach (ConstructorInfo ctorInfo in constructorInfos)
                {
                    try
                    {
                        ParameterInfo[] parameters = ctorInfo.GetParameters();
                        object[] paramInstances = new object[parameters.Length];

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            Type t = parameters[i].ParameterType;
                            paramInstances[i] = serviceProvider.GetInstance(t);
                        }

                        instance = ctorInfo.Invoke(paramInstances);
                        this.instance = instance;
                        return true;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                return false; // If we don't return from loop then we couldn't construct instance.
            }
            else
            {
                // Lastly assume factory delegate.
                if (factoryDelegate is null)
                {
                    return false;
                }

                instance = factoryDelegate.Invoke(serviceProvider);
                return true;
            }
        }
    }
}
