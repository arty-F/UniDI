using System;

namespace MonoInjector
{
    public class MonoInjectorException : Exception
    {
        public MonoInjectorException() { }
        public MonoInjectorException(string message) : base(message) { }
        public MonoInjectorException(string message, Exception innerException) : base(message, innerException) { }
    }
}
