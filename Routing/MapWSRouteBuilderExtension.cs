using System;
using Weerly.WebSocketWrapper.Processing;
using Weerly.WebSocketWrapper.Routing;
using static Weerly.WebSocketWrapper.Enums.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Builder
{
    public static class WebSocketRouteBuilderExtension
    {
        public static System.String CommonClass = "WebSocket";

        public static CommonType CommonType = CommonType.Controller;
        public static IWebSocketRouteBuilder MapWSRouteAsync(this IWebSocketRouteBuilder wsRouteBuilder, string name, string template)
        {
            if (!wsRouteBuilder.ContextPathFound)
            {
                CommonType type = GetDefaultType(wsRouteBuilder);
                String classNamespace = GetDefaultClassNamespace(wsRouteBuilder);
                IWebSocketRouter Router = new WebSocketRouter(name, template, type, classNamespace);
                IWebSocketRouteHandler routeProcessor = wsRouteBuilder.RouteHandler;
                routeProcessor.VerifyRouteData(Router).AddRouteData(Router).Build(wsRouteBuilder);
            }

            return wsRouteBuilder;
        }
        public static IWebSocketRouteBuilder MapWSRoute(this IWebSocketRouteBuilder wsRouteBuilder, string name, string template, CommonType type)
        {
            if (!wsRouteBuilder.ContextPathFound)
            {
                System.String classNamespace = GetDefaultClassNamespace(wsRouteBuilder);
                IWebSocketRouter Router = new WebSocketRouter(name, template, type, classNamespace);
                IWebSocketRouteHandler routeProcessor = wsRouteBuilder.RouteHandler;
                routeProcessor.VerifyRouteData(Router).AddRouteData(Router).Build(wsRouteBuilder);
            }

            return wsRouteBuilder;
        }
        public static IWebSocketRouteBuilder MapWSRoute(this IWebSocketRouteBuilder wsRouteBuilder, string name, string template, CommonType type, string classNamespace)
        {
            if (!wsRouteBuilder.ContextPathFound)
            {
                IWebSocketRouter Router = new WebSocketRouter(name, template, type, classNamespace);
                IWebSocketRouteHandler routeProcessor = wsRouteBuilder.RouteHandler;
                routeProcessor.VerifyRouteData(Router).AddRouteData(Router).Build(wsRouteBuilder);
            }

            return wsRouteBuilder;
        }

        public static CommonType GetDefaultType(IWebSocketRouteBuilder wsRouteBuilder)
        {
            CommonType DefaultType = CommonType;

            if (!wsRouteBuilder.CommonType.Equals(null))
            {
                DefaultType = wsRouteBuilder.CommonType;
            }

            return DefaultType;
        }
        public static String GetDefaultClassNamespace(IWebSocketRouteBuilder wsRouteBuilder)
        {
            String DefaultClassLocation = null;

            if (!wsRouteBuilder.CommonType.Equals(null))
            {
                if (wsRouteBuilder.CommonType.Equals(CommonType.Class))
                {
                    if (!String.IsNullOrWhiteSpace(wsRouteBuilder.CommonClassNamespace))
                    {
                        DefaultClassLocation = wsRouteBuilder.CommonClassNamespace;
                    }
                    else
                    {
                        throw new Exception("class namespace not found");
                    }
                }
            }

            return DefaultClassLocation;
        }
    }
}
