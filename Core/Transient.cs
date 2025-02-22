using System.Reflection;

namespace HMServiceProvider.Core
{
    /// <summary>
    /// Represents a transient instance.
    /// </summary>
    internal class Transient : ObjectInstance
    {
        public Transient(Type type) : base(type) { }

        public Transient(Func<ServiceProvider, object> factory) : base(factory) { }

        public override bool TryGetInstance(ServiceProvider serviceProvider, out object? instance)
        {
            instance = null;

            if (constructorInfos is not null)
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
