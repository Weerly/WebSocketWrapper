using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    class WrongReturnedTypeException : Exception
    {
        public WrongReturnedTypeException() : base("method return an unexpected result type")
        {
        }
    }
}
