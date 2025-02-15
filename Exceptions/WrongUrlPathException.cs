using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    public class WrongUrlPathException: Exception
    {
        public WrongUrlPathException() : base("Url path didn't match any path types in the system")
        {
        }
    }
}
