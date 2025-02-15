using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    public class WrongReturnedTypeException : Exception
    {
        public WrongReturnedTypeException() : base("method return an unexpected result type")
        {
        }
    }
}
