using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Weerly.WebSocketWrapper.Abstractions;
using Weerly.WebSocketWrapper.Processing;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Routing
{
    /// <summary>
    /// Provides a builder for configuring WebSocket routes in an HTTP context.
    /// </summary>
    /// <remarks>
    /// This class is responsible for managing WebSocket routing logic by holding route configurations
    /// and handling the matching of URLs to their corresponding WebSocket routers.
    /// </remarks>
    /// <example>
    /// This class can be instantiated directly with an HTTP context and optionally with additional configuration parameters such as
    /// common type, class name, and class namespace.
    /// </example>
    public class WebSocketRouteBuilder : IWebSocketRouteBuilder
    {
        public CommonType CommonType { get; }
        public string CommonClassNamespace { get; }
        public string CommonClass { get; }
        public string RootNamespace { get; }
        public IWebSocketRouteHandler RouteHandler { get; }
        public IList<IWebSocketRouter> Routes { get; }
        public HttpContext Context { get; }
        public Boolean MatchUrlPath { get; set; }
        public Boolean ContextPathFound { get; set; }
        protected IWebSocketRouter Router { get; set; }

        /// <summary>
        /// Implements functionality to build and manage WebSocket routes in an ASP.NET context.
        /// </summary>
        /// <remarks>
        /// This class provides mechanisms to configure WebSocket routing based on the HTTP context. It supports various customization options like the type of WebSocket connection and namespace or class-based filtering for routes.
        /// The class internally holds a collection of WebSocket routers to process and handle incoming WebSocket requests.
        /// </remarks>
        public WebSocketRouteBuilder(HttpContext context)
        {
            ContextPathFound = false;
            Context = context;
            Routes = new List<IWebSocketRouter>();
            CommonType = CommonType.Controller;
            CommonClass = "WebSocket";
            RouteHandler = new WebSocketRouteHandler(Routes, CommonClass);
        }

        /// <summary>
        /// Provides functionality to build and manage WebSocket routes within an HTTP context for an ASP.NET application.
        /// </summary>
        /// <remarks>
        /// The class handles the configuration of WebSocket routes, including the association of HTTP context paths with WebSocket routers.
        /// It supports customizable routing properties, such as the type of WebSocket connection, and allows integration with user-defined namespaces and classes.
        /// Internally, it manages a collection of route configurations and helps to process incoming WebSocket requests by leveraging registered handlers.
        /// </remarks>
        public WebSocketRouteBuilder(HttpContext context, CommonType commonType)
        {
            ContextPathFound = false;
            Context = context;
            Routes = new List<IWebSocketRouter>();
            CommonType = commonType;
            CommonClass = "WebSocket";
            RouteHandler = new WebSocketRouteHandler(Routes, CommonClass);
        }

        /// <summary>
        /// Facilitates the creation and management of WebSocket route configurations for an ASP.NET application.
        /// </summary>
        /// <remarks>
        /// This class serves as a mechanism to define and organize WebSocket routes by leveraging an HTTP context. It supports customization options such as specifying common types, classes, and namespaces for the routes. The class holds a collection of WebSocket routers and provides the logic to handle route matching and processing for WebSocket requests.
        /// </remarks>
        public WebSocketRouteBuilder(HttpContext context, CommonType commonType, string commonClass)
        {
            ContextPathFound = false;
            Context = context;
            Routes = new List<IWebSocketRouter>();
            CommonType = commonType;
            CommonClass = commonClass;
            RouteHandler = new WebSocketRouteHandler(Routes, CommonClass);
        }

        /// <summary>
        /// Provides the capability to configure and manage WebSocket routes within a specified HTTP context.
        /// </summary>
        /// <remarks>
        /// This class facilitates the construction of WebSocket routing rules by leveraging user-specified parameters
        /// such as the HTTP context, common class names, and namespaces, as well as configurable options such
        /// as route matching and contextual path detection. It maintains an internal collection of WebSocket routers
        /// for routing and processing WebSocket connections.
        /// </remarks>
        public WebSocketRouteBuilder(HttpContext context, CommonType commonType, string commonClass, string commonClassNamespace)
        {
            ContextPathFound = false;
            Context = context;
            Routes = new List<IWebSocketRouter>();
            CommonType = commonType;
            CommonClass = commonClass;
            CommonClassNamespace = commonClassNamespace;
            RouteHandler = new WebSocketRouteHandler(Routes, CommonClass);
        }
    }
}