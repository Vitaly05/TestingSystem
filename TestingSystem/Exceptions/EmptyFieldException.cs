using System;

namespace TestingSystem.Exceptions
{
    internal class EmptyFieldException : Exception
    {
        public EmptyFieldException() 
            : base() { }

        public EmptyFieldException(string message)
            : base(message) { }
    }
}
