using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Weerly.WebSocketWrapper.Processing;
using static Weerly.WebSocketWrapper.Enums.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Routing
{
    public class WebSocketRouteBuilder : IWebSocketRouteBuilder
    {
        public CommonType CommonType { get; }
        public String CommonClassNamespace { get; }
        public String CommonClass { get; }
        public String RootNamespace { get; }
        public IWebSocketRouteHandler RouteHandler { get; }
        public IList<IWebSocketRouter> Routes { get; }
        public HttpContext Context { get; }
        public Boolean MatchUrlPath { get; set; }
        public Boolean ContextPathFound { get; set; }
        protected IWebSocketRouter Router { get; set; }

        public WebSocketRouteBuilder(HttpContext context)
        {
            ContextPathFound = false;
            Context = context;
            Routes = new List<IWebSocketRouter>();
            CommonType = CommonType.Controller;
            CommonClass = "WebSocket";
            RouteHandler = new WebSocketRouteHandler(Routes, CommonClass);
        }

        public WebSocketRouteBuilder(HttpContext context, CommonType commonType)
        {
            ContextPathFound = false;
            Context = context;
            Routes = new List<IWebSocketRouter>();
            CommonType = commonType;
            CommonClass = "WebSocket";
            RouteHandler = new WebSocketRouteHandler(Routes, CommonClass);
        }

        public WebSocketRouteBuilder(HttpContext context, CommonType commonType, string commonClass)
        {
            ContextPathFound = false;
            Context = context;
            Routes = new List<IWebSocketRouter>();
            CommonType = commonType;
            CommonClass = commonClass;
            RouteHandler = new WebSocketRouteHandler(Routes, CommonClass);
        }

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