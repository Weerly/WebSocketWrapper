using System.Collections.Generic;
using Weerly.WebSocketWrapper.Processing;
using Weerly.WebSocketWrapper.Routing;

namespace Weerly.WebSocketWrapper.Abstractions
{
    /// <summary>
    /// Represents a collection of methods for handling routes
    /// </summary>
    public interface IWebSocketRouteHandler
    {
        IList<IWebSocketRouter> Routes { get; }
        IWebSocketRouter Router { get; set; }
        IWebSocketRouteHandler VerifyRouteData(IWebSocketRouter router);
        IWebSocketRouteHandler AddRouteData(IWebSocketRouter router);
        void Build(IWebSocketRouteBuilder builder);
    }
}
