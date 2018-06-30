using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    class WrongUrlPathException: Exception
    {
        public WrongUrlPathException() : base("Url path didn`t match any path types in the system")
        {
        }
    }
}
