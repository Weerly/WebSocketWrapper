using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Weerly.WebSocketWrapper.Abstractions;
using Weerly.WebSocketWrapper.Exceptions;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Processing
{
    /// <summary>
    /// Represents a collection of methods for handling routes
    /// </summary>
    class WebSocketRouteHandler : IWebSocketRouteHandler
    {
        /// <summary>
        /// Gets a collection of routes in IWebSocketRouter
        /// </summary>
        /// <returns>IList collection of </returns>
        public IList<IWebSocketRouter> Routes { get; set; }

        private bool MatchUrlPath { get; set; }
        public IWebSocketRouter Router { get; set; }
        private string RootNamespace { get; }
        private string CommonClass { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebSocketRouteHandler"/> class.
        /// </summary>
        /// <param name="routes">The list of WebSocket routers.</param>
        /// <param name="commonClass">The common class name used in routing.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="commonClass"/> is null or whitespace.</exception>
        public WebSocketRouteHandler(IList<IWebSocketRouter> routes, string commonClass)
        {
            if (string.IsNullOrWhiteSpace(commonClass))
            {
                throw new ArgumentException("message", nameof(commonClass));
            }

            CommonClass = commonClass;
            RootNamespace = Assembly.GetEntryAssembly()?.GetName().Name;
            Routes = routes;
        }
        
        /// <summary>
        /// Verifies the route data for the given WebSocket router.
        /// </summary>
        /// <param name="router">The WebSocket router to verify.</param>
        /// <returns>Returns the current instance of the IWebSocketRouteHandler.</returns>
        /// <exception cref="DuplicateNameException">Thrown if a route with the same name as the router already exists.</exception>
        public IWebSocketRouteHandler VerifyRouteData(IWebSocketRouter router)
        {
            
            MatchUrlPath = false;

            if (Routes.Count > 0)
            {
                foreach (var item in Routes)
                {
                    if (item.Name.Equals(router.Name))
                    {
                        throw new DuplicateNameException();
                    }
                }

            }

            ParseTemplate(router);

            return this;
        }
        
        /// <summary>
        /// Adds a WebSocket router to the route handler if the URL path matches.
        /// </summary>
        /// <param name="router">The WebSocket router to add.</param>
        /// <returns>Returns the current WebSocket route handler.</returns>
        public IWebSocketRouteHandler AddRouteData(IWebSocketRouter router)
        {
            if (MatchUrlPath)
            {
                Routes.Add(router);
                Router = router;
            }

            return this;
        }

        /// <summary>
        /// Builds the WebSocket route using the given route builder.
        /// </summary>
        /// <param name="builder">The WebSocket route builder.</param>
        /// <exception cref="WrongUrlPathException">Thrown when the URL path does not match.</exception>
        public void Build(IWebSocketRouteBuilder builder)
        {
            if (!MatchUrlPath)
            {
                throw new WrongUrlPathException();
            }

            if (!builder.Context.Request.Path.Equals(Router.PathName)) return;
            builder.ContextPathFound = true;

            if (!builder.Context.WebSockets.IsWebSocketRequest)
            {
                builder.Context.Response.StatusCode = 400;
            }
        }
        /// <summary>
        /// Parses the template from the given WebSocket router and extracts relevant data.
        /// </summary>
        /// <param name="router">The WebSocket router containing the template and patterns.</param>
        /// <remarks>
        /// This method iterates over the patterns in the router and attempts to match them against the template.
        /// If a match is found, it extracts the URL type, constructs the handlers array, and updates the router's
        /// action data accordingly.
        /// </remarks>
        private void ParseTemplate(IWebSocketRouter router)
        {
            MatchUrlPath = false;
            var template = router.Template;
            var patterns = router.Patterns;
            var index = 0;

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(template, pattern);

                if (match.Success)
                {
                    var urlType = router.Type.ToString() ?? router.Types[index];
                    var len = match.Groups.Count;
                    var handlers = new string[len];
                    handlers[0] = "/";

                    for (var i = 1; i < len; i++)
                    {
                        handlers[0] += match.Groups[i].Value;
                        handlers[0] += ((i == 1) && (len > 2)) ? "/" : "";
                        handlers[i] = match.Groups[i].Value;
                    }

                    router.PathName = handlers[0];

                    router.ActionData.Add("type", urlType);
                    router.ActionData.Add("className", (handlers.Length > 2) ? handlers[1] : null);
                    router.ActionData.Add("methodName", (handlers.Length > 2) ? handlers[2] : handlers[1]);
                    router.ActionData.Add("object", GetObjectName(urlType, router));

                    MatchUrlPath = true;
                    break;
                }

                index++;
            }
        }

        /// <summary>
        /// Gets the object name based on the provided URL type and WebSocket router.
        /// </summary>
        /// <param name="urlType">The type of the URL, which dictates how the object name is constructed.</param>
        /// <param name="router">The WebSocket router containing the action data and namespace information.</param>
        /// <returns>Returns the constructed object name as a string.</returns>
        /// <exception cref="Exception">Thrown if the class name is empty or the object name cannot be determined.</exception>
        private string GetObjectName(string urlType, IWebSocketRouter router)
        {
            var ObjectName = "";
            var className = router.ActionData["className"];

            if (string.IsNullOrWhiteSpace(className))
            {
                if (!string.IsNullOrWhiteSpace(CommonClass))
                {
                    className = CommonClass;
                }
                else
                {
                    throw new Exception("class name should not be empty");
                }
            }

            if (urlType.Equals(CommonType.Controller.ToString()))
            {
                ObjectName = RootNamespace + ".Controllers." + className + "Controller";
            }
            else if (urlType.Equals(CommonType.Class.ToString()))
            {
                var rootNamespace = router.GetNamespace(RootNamespace, router.ClassNamespace);
                ObjectName = $"{rootNamespace}.{className}";
            }
            else
            {
                throw new Exception("object name should not be empty");
            }

            return ObjectName;
        }
    }
}
