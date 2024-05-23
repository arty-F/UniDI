using System;

namespace MonoInjector
{
    public class UniDIException : Exception
    {
        public UniDIException() { }
        public UniDIException(string message) : base(message) { }
        public UniDIException(string message, Exception innerException) : base(message, innerException) { }
    }
}
