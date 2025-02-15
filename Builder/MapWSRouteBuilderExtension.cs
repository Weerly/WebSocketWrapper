using System;
using Weerly.WebSocketWrapper.Abstractions;
using Weerly.WebSocketWrapper.Routing;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Builder
{
    /// <summary>
    /// Provides extension methods for configuring WebSocket routes.
    /// </summary>
    public static class WebSocketRouteBuilderExtension
    {
        static WebSocketRouteBuilderExtension()
        {
        }

        /// <summary>
        /// Asynchronously maps a WebSocket route with a specified name and template.
        /// </summary>
        /// <param name="wsRouteBuilder">The WebSocket route builder.</param>
        /// <param name="name">The name of the WebSocket route.</param>
        /// <param name="template">The template for the WebSocket route.</param>
        /// <returns>The updated WebSocket route builder.</returns>
        public static IWebSocketRouteBuilder MapWsRouteAsync(this IWebSocketRouteBuilder wsRouteBuilder, string name, string template)
        {
            if (wsRouteBuilder.ContextPathFound) return wsRouteBuilder;
            var type = GetDefaultType(wsRouteBuilder);
            var classNamespace = GetDefaultClassNamespace(wsRouteBuilder);
            OperateWebSocketRouter(wsRouteBuilder, name, template, type, classNamespace, true);

            return wsRouteBuilder;
        }
        
        /// <summary>
        /// Asynchronously maps a WebSocket route with a specified name and template.
        /// </summary>
        /// <param name="wsRouteBuilder">The WebSocket route builder.</param>
        /// <param name="name">The name of the WebSocket route.</param>
        /// <param name="template">The template for the WebSocket route.</param>
        /// /// <param name="type">The common type for the WebSocket route.</param>
        /// <param name="classNamespace">The class namespace for the WebSocket route.</param>
        /// <returns>The updated WebSocket route builder.</returns>
        public static IWebSocketRouteBuilder MapWsRouteAsync(this IWebSocketRouteBuilder wsRouteBuilder, string name, string template, CommonType type, string classNamespace)
        {
            if (wsRouteBuilder.ContextPathFound) return wsRouteBuilder;
            OperateWebSocketRouter(wsRouteBuilder, name, template, type, classNamespace, true);

            return wsRouteBuilder;
        }
        
        /// <summary>
        /// Maps a WebSocket route with a specified name, template, and type.
        /// </summary>
        /// <param name="wsRouteBuilder">The WebSocket route builder.</param>
        /// <param name="name">The name of the WebSocket route.</param>
        /// <param name="template">The template for the WebSocket route.</param>
        /// <param name="type">The common type for the WebSocket route.</param>
        /// <returns>The updated WebSocket route builder.</returns>
        public static IWebSocketRouteBuilder MapWsRoute(this IWebSocketRouteBuilder wsRouteBuilder, string name, string template, CommonType type)
        {
            if (wsRouteBuilder.ContextPathFound) return wsRouteBuilder;
            var classNamespace = GetDefaultClassNamespace(wsRouteBuilder);
            OperateWebSocketRouter(wsRouteBuilder, name, template, type, classNamespace, false);

            return wsRouteBuilder;
        }
        
        /// <summary>
        /// Maps a WebSocket route with a specified name, template, type, and class namespace.
        /// </summary>
        /// <param name="wsRouteBuilder">The WebSocket route builder.</param>
        /// <param name="name">The name of the WebSocket route.</param>
        /// <param name="template">The template for the WebSocket route.</param>
        /// <param name="type">The common type for the WebSocket route.</param>
        /// <param name="classNamespace">The class namespace for the WebSocket route.</param>
        /// <returns>The updated WebSocket route builder.</returns>
        public static IWebSocketRouteBuilder MapWsRoute(this IWebSocketRouteBuilder wsRouteBuilder, string name, string template, CommonType type, string classNamespace)
        {
            if (wsRouteBuilder.ContextPathFound) return wsRouteBuilder;
            OperateWebSocketRouter(wsRouteBuilder, name, template, type, classNamespace, false);

            return wsRouteBuilder;
        }

        /// <summary>
        /// Retrieves the default common type for the WebSocket route builder.
        /// </summary>
        /// <param name="wsRouteBuilder">The WebSocket route builder.</param>
        /// <returns>The default common type.</returns>
        private static CommonType GetDefaultType(IWebSocketRouteBuilder wsRouteBuilder)
        {
            return wsRouteBuilder?.CommonType ?? CommonType.Controller;
        }

        /// <summary>
        /// Retrieves the default class namespace for the WebSocket route builder.
        /// </summary>
        /// <param name="wsRouteBuilder">The WebSocket route builder.</param>
        /// <returns>The default class namespace.</returns>
        /// <exception cref="Exception">Thrown when the class namespace is not found.</exception>
        private static string GetDefaultClassNamespace(IWebSocketRouteBuilder wsRouteBuilder)
        {
            string defaultClassLocation;

            if (!wsRouteBuilder.CommonType.Equals(CommonType.Class)) return null;
            if (!string.IsNullOrWhiteSpace(wsRouteBuilder.CommonClassNamespace))
            {
                defaultClassLocation = wsRouteBuilder.CommonClassNamespace;
            }
            else
            {
                throw new Exception("Class namespace not found");
            }

            return defaultClassLocation;
        }

        /// <summary>
        /// Creates and configures a WebSocket router and adds it to the route handler.
        /// </summary>
        /// <param name="wsRouteBuilder">The WebSocket route builder.</param>
        /// <param name="name">The name of the WebSocket route.</param>
        /// <param name="template">The template for the WebSocket route.</param>
        /// <param name="type">The common type for the WebSocket route.</param>
        /// <param name="classNamespace">The class namespace for the WebSocket route.</param>
        /// <param name="isAsync">identify is method needs to be async</param>
        private static void OperateWebSocketRouter(IWebSocketRouteBuilder wsRouteBuilder, string name, string template,
            CommonType type, string classNamespace, bool isAsync)
        {
            IWebSocketRouter router = new WebSocketRouter(name, template, type, classNamespace, isAsync);
            var routeProcessor = wsRouteBuilder.RouteHandler;
            routeProcessor?.VerifyRouteData(router).AddRouteData(router).Build(wsRouteBuilder);
        }
    }
}
