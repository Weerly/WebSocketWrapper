using System.Collections.Generic;
using System.Threading.Tasks;
using Weerly.WebSocketWrapper.Routing;

namespace Weerly.WebSocketWrapper.Processing
{
    /// <summary>
    /// Represents a collection of methods for handling routes
    /// </summary>
    public interface IWebSocketRouteHandler
    {
        IList<IWebSocketRouter> Routes { get; }
        IWebSocketRouter Router { get; set; }
        IWebSocketRouteHandler VerifyRouteData(IWebSocketRouter Router);
        IWebSocketRouteHandler AddRouteData(IWebSocketRouter Router);
        void Build(IWebSocketRouteBuilder builder);
    }
}
