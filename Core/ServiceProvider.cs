namespace HMServiceProvider.Core
{
    /// <summary>
    /// A home made service provider.
    /// Mostly used for school projects where external packages are forbidden.
    /// </summary>
    public class ServiceProvider
    {
        private Dictionary<Type, IObjectInstance> instances;

        public ServiceProvider()
        {
            instances = new Dictionary<Type, IObjectInstance>();
        }

        /// <summary>
        /// Register a singleton instance to the service provider.
        /// </summary>
        /// <exception cref="InstanceAlreadyRegisteredException">If an instance of the declared type is already registered.</exception>
        public void RegisterSingleton<T>()
        {
            if (instances.ContainsKey(typeof(T)))
            {
                throw new InstanceAlreadyRegisteredException();
            }
            else
            {
                instances.Add(typeof(T), new Singleton(typeof(T)));
            }
        }

        /// <summary>
        /// Register a singleton instance to the service provider with specified factory delegate.
        /// </summary>
        /// <exception cref="InstanceAlreadyRegisteredException">If an instance of the declared type is already registered.</exception>
        public void RegisterSingleton<T>(Func<ServiceProvider, T> factoryDelegate)
        {
            if (instances.ContainsKey(typeof(T)))
            {
                throw new InstanceAlreadyRegisteredException();
            }
            else
            {
                // Bit of a hack, but I wanna avoid generics in object instances.
                instances.Add(typeof(T), new Singleton(serviceProvider =>
                {
                    object? instance = factoryDelegate.Invoke(serviceProvider);
                    return instance is T typedInstance ? (object)typedInstance : throw new UnregisteredInstanceException();
                }));
            }
        }

        /// <summary>
        /// Register a transient instance to the service provider.
        /// </summary>
        /// <exception cref="InstanceAlreadyRegisteredException">If an instance of the declared type is already registered.</exception>
        public void RegisterTransient<T>()
        {
            if (instances.ContainsKey(typeof(T)))
            {
                throw new InstanceAlreadyRegisteredException();
            }
            else
            {
                instances.Add(typeof(T), new Transient(typeof(T)));
            }
        }

        /// <summary>
        /// Register a transient instance to the service provider with specified factory delegate.
        /// </summary>
        /// <exception cref="InstanceAlreadyRegisteredException">If an instance of the declared type is already registered.</exception>
        public void RegisterTransient<T>(Func<ServiceProvider, T> factoryDelegate)
        {
            if (instances.ContainsKey(typeof(T)))
            {
                throw new InstanceAlreadyRegisteredException();
            }
            else
            {
                // Bit of a hack, but I wanna avoid generics in object instances.
                instances.Add(typeof(T), new Transient(serviceProvider =>
                {
                    object? instance = factoryDelegate.Invoke(serviceProvider);
                    return instance is T typedInstance ? (object)typedInstance : throw new UnregisteredInstanceException();
                }));
            }
        }

        /// <summary>
        /// Get registered instance from service provider.
        /// </summary>
        /// <exception cref="UnregisteredInstanceException">If instance is not registered, or could not be accessed for some reason.</exception>
        public T GetInstance<T>()
        {
            try
            {
                return instances.TryGetValue(typeof(T), out IObjectInstance? i)
                && i.TryGetInstance(this, out object? o)
                && o is T instance
                ? instance
                : throw new UnregisteredInstanceException();
            }
            catch (Exception)
            {
                throw new UnregisteredInstanceException();
            }
        }

        /// <summary>
        /// Only used by internal instances.
        /// </summary>
        internal object GetInstance(Type type)
        {
            return instances.TryGetValue(type, out IObjectInstance? i)
                && i.TryGetInstance(this, out object? o) && o is not null
                ? o : throw new UnregisteredInstanceException();
        }
    }
}
