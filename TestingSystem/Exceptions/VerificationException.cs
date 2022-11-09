using System;

namespace TestingSystem.Exceptions
{
    [Serializable]
    internal class VerificationException : Exception
    {
        public VerificationException() { }

        public VerificationException(string message) 
            : base(message) { }
    }
}
