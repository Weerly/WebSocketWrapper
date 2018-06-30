using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Weerly.WebSocketWrapper.Processing;
using static Weerly.WebSocketWrapper.Enums.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Routing
{
    public interface IWebSocketRouteBuilder
    {
        //
        // summery:
        //     Adds a root namespace, common class namespace, common class type and route (with the specified
        //     name, template and if it needs type and class namespace) to the Weerly.WebSocketWrapper.Routing.IWebSocketRouteBuilder
        //
        // Параметры:
        //   IWebSocketRouteBuilder:
        //     The Microsoft.AspNetCore.Routing.IRouteBuilder to add the route to.
        //
        //   root
        //   name:
        //     The name of the route.
        //
        //   template:
        //     The URL pattern of the route.
        //
        // Возврат:
        //     A reference to this instance after the operation has completed.
        
        HttpContext Context { get; }
        IWebSocketRouteHandler RouteHandler { get; }
        Boolean ContextPathFound { get; set; }
        CommonType CommonType { get; }
        String CommonClass { get; }
        String CommonClassNamespace { get; }
        IList<IWebSocketRouter> Routes { get; }

    }
}
