using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    public class DuplicateNameException : Exception
    {
        public DuplicateNameException() : base("Route name should not be duplicated")
        {
        }
    }
}
