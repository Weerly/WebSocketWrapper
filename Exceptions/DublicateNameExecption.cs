using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    class DublicateNameException : Exception
    {
        public DublicateNameException() : base("Route name should not be dublicated")
        {
        }
    }
}
