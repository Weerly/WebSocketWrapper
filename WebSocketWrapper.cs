using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Weerly.WebSocketWrapper.Processing;
using Weerly.WebSocketWrapper.Routing;
using static Weerly.WebSocketWrapper.Enums.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Builder
{
    public static class WebSocketWrapper
    {
        /// <summary>
        /// dfdfdfdfdfdfddfsdfsdfsd
        /// </summary>
        /// <param name="app"></param>
        /// <param name="rootNamespace"></param>
        /// <param name="configureRoutes"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebSocketRoutes(this IApplicationBuilder app, Action<IWebSocketRouteBuilder> configureRoutes)
        {
            return app.UseWebSocketRoutes(configureRoutes, CommonType.Controller, "WebSocket");
        }
        public static IApplicationBuilder UseWebSocketRoutes(this IApplicationBuilder app, Action<IWebSocketRouteBuilder> configureRoutes, string commonClassName)
        {
            return app.UseWebSocketRoutes(configureRoutes, CommonType.Controller, commonClassName);
        }
        public static IApplicationBuilder UseWebSocketRoutes(this IApplicationBuilder app, Action<IWebSocketRouteBuilder> configureRoutes, CommonType commonType, string commonClassName)
        {
            return app.UseWebSocketRoutes(configureRoutes, commonType, commonClassName, null);
        }
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