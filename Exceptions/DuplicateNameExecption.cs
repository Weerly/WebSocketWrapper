using System;

namespace Weerly.WebSocketWrapper.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a duplicate route name is detected.
    /// </summary>
    /// <remarks>
    /// This exception is typically used in scenarios where a router validation process, such as in a WebSocket routing system, identifies
    /// duplicate route names that must remain unique for correct operation.
    /// </remarks>
    public class DuplicateNameException : Exception
    {
        public DuplicateNameException() : base("Route name should not be duplicated")
        {
        }
    }
}
