using System;
using Microsoft.AspNetCore.Builder;
using Weerly.WebSocketWrapper.Abstractions;
using Weerly.WebSocketWrapper.Processing;
using Weerly.WebSocketWrapper.Routing;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper
{
    /// <summary>
    /// Provides extension methods for configuring WebSocket routes in an application.
    /// </summary>
    public static class WebSocketWrapper
    {
        /// <summary>
        /// Configures WebSocket routes using a default common type and class name.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configureRoutes">The action to configure routes.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseWebSocketRoutes(this IApplicationBuilder app, Action<IWebSocketRouteBuilder> configureRoutes)
        {
            return app.UseWebSocketRoutes(configureRoutes, CommonType.Controller, "WebSocket");
        }
        
        /// <summary>
        /// Configures WebSocket routes using a specified common class name.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configureRoutes">The action to configure routes.</param>
        /// <param name="commonClassName">The common class name for WebSocket routes.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseWebSocketRoutes(this IApplicationBuilder app, Action<IWebSocketRouteBuilder> configureRoutes, string commonClassName)
        {
            return app.UseWebSocketRoutes(configureRoutes, CommonType.Controller, commonClassName);
        }
        
        /// <summary>
        /// Configures WebSocket routes using a specified common type and class name.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configureRoutes">The action to configure routes.</param>
        /// <param name="commonType">The common type for WebSocket routes.</param>
        /// <param name="commonClassName">The common class name for WebSocket routes.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseWebSocketRoutes(this IApplicationBuilder app, Action<IWebSocketRouteBuilder> configureRoutes, CommonType commonType, string commonClassName)
        {
            return app.UseWebSocketRoutes(configureRoutes, commonType, commonClassName, null);
        }
        
        /// <summary>
        /// Configures WebSocket routes using a specified common type, class name, and class namespace.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="configureRoutes">The action to configure routes.</param>
        /// <param name="commonType">The common type for WebSocket routes.</param>
        /// <param name="commonClassName">The common class name for WebSocket routes.</param>
        /// <param name="classNamespace">The namespace of the class for WebSocket routes.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseWebSocketRoutes(this IApplicationBuilder app, Action<IWebSocketRouteBuilder> configureRoutes, CommonType commonType, string commonClassName, string classNamespace)
        {
            app.Use(async (context, next) =>
            {
                IWebSocketRouteBuilder wsRouteBuilder = new WebSocketRouteBuilder(context, commonType, commonClassName, classNamespace);
                configureRoutes(wsRouteBuilder);

                if (wsRouteBuilder.ContextPathFound)
                {
                    IWebSocketRouteHandler routeProcessor = wsRouteBuilder.RouteHandler;

                    await DataProcessing.ConnectionProcess(wsRouteBuilder);
                }
                else
                {
                    await next();
                }
            });

            return app;
        }
    }
}