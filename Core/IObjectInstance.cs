namespace HMServiceProvider.Core
{
    /// <summary>
    /// Represents a general object instance.
    /// </summary>
    internal interface IObjectInstance
    {
        /// <summary>
        /// Try to access an instance of the underlying type.
        /// </summary>
        bool TryGetInstance(ServiceProvider serviceProvider, out object? instance);
    }
}
