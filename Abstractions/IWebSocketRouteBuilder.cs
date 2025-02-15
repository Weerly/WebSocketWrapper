using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Abstractions
{
    /// <summary>
    /// Interface for defining WebSocket route builder functionality.
    /// Used to create and manage WebSocket route configurations.
    /// </summary>
    public interface IWebSocketRouteBuilder
    {
        HttpContext Context { get; }
        IWebSocketRouteHandler RouteHandler { get; }
        Boolean ContextPathFound { get; set; }
        CommonType CommonType { get; }
        string CommonClass { get; }
        string CommonClassNamespace { get; }
        IList<IWebSocketRouter> Routes { get; }

    }
}
