namespace HMServiceProvider.Core
{
    public class UnregisteredInstanceException : Exception
    {
        public UnregisteredInstanceException() : base() { }
        public UnregisteredInstanceException(string message) : base(message) { }
    }
}
