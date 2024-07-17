using System;

namespace UniDI
{
    public class UniDIException : Exception
    {
        public UniDIException() { }
        public UniDIException(string message) : base(message) { }
        public UniDIException(string message, Exception innerException) : base(message, innerException) { }
    }
}
