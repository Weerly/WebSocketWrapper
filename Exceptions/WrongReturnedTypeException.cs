using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a method returns an unexpected result type.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown to indicate that a method's return type does not match the expected type,
    /// which can occur during reflection-based dynamic type checking or method validation.
    /// </remarks>
    public class WrongReturnedTypeException : Exception
    {
        public WrongReturnedTypeException() : base("method return an unexpected result type")
        {
        }
    }
}
