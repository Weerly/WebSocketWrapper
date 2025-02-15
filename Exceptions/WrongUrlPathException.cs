using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a URL path does not match any defined path types in the system.
    /// </summary>
    /// <remarks>
    /// This exception is typically used within the WebSocket routing context to indicate that the URL path specified in the request does not
    /// correspond to any configured WebSocket route paths.
    /// </remarks>
    public class WrongUrlPathException: Exception
    {
        public WrongUrlPathException() : base("Url path didn't match any path types in the system")
        {
        }
    }
}
