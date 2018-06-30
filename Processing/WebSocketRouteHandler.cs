using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Weerly.WebSocketWrapper.Exceptions;
using Weerly.WebSocketWrapper.Routing;
using static Weerly.WebSocketWrapper.Enums.WebSocketEnums;

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
        public Boolean MatchUrlPath { get; private set; }
        public IWebSocketRouter Router { get; set; }
        public String RootNamespace { get; }
        public String CommonClass { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="rootNamespace"></param>
        /// <exception cref="ArgumentException">rootNamespace should not be empty, null or consists only of whitespaces characters </exception>
        /// <exception cref="ArgumentException">commonClass should not be empty, null or consists only of whitespaces characters </exception>
        public WebSocketRouteHandler(IList<IWebSocketRouter> routes, string commonClass)
        {
            if (string.IsNullOrWhiteSpace(commonClass))
            {
                throw new ArgumentException("message", nameof(commonClass));
            }
            else
            {
                CommonClass = commonClass;
                RootNamespace = Assembly.GetEntryAssembly().GetName().Name;
            }

            Routes = routes;
        }
        /// <summary>
        /// dssdsdsds
        /// </summary>
        /// <param name="router"></param>
        public void ParseTemplate(IWebSocketRouter router)
        {
            MatchUrlPath = false;
            string[] handlers = new string[0];
            string template = router.Template;
            String[] patterns = router.Patterns;
            int index = 0;

            foreach (var pattern in patterns)
            {
                Match match = Regex.Match(template, pattern);

                if (match.Success)
                {
                    String urlType = router.Types[index];
                    int len = match.Groups.Count;
                    handlers = new string[len];
                    handlers[0] = "/";

                    for (int i = 1; i < len; i++)
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
        ///     Formats Object Name using specified type of class, name of class and common class name if need it
        /// </summary>
        /// <param name="urlType">type of class will be invoked(class or controller)</param>
        /// <param name="router">instance of IWebSocketRouter which consists data about current route </param>
        /// <param name="commonClass">common name of invoking object`s class</param>
        /// <returns>String</returns>
        /// <exception cref="T:System.Exception">router.ActionData["className"] or commonClass is null, empty or consists only of white-space characters</exception>
        /// <exception cref="T:System.Exception">resulted object`s name is null, empty or consists only of white-space</exception>
        /// 
        public String GetObjectName(string urlType, IWebSocketRouter router)
        {
            String ObjectName = "";
            String className = router.ActionData["className"];

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
                String Namespace = router.GetNamespace((string)RootNamespace, (string)router.ClassNamespace);
                ObjectName = Namespace + className;
            }
            else
            {
                throw new Exception("object name should not be empty");
            }

            return ObjectName;
        }

        public IWebSocketRouteHandler VerifyRouteData(IWebSocketRouter Router)
        {
            //
            // summary:
            //     Formats Object Name using specified type of class, name of class and common class name if need it
            //
            // parameters:
            //   urlType:
            //     type of class will be invoked(class or controller)
            //
            //   router:
            //     instance of IWebSocketRouter which consists data about current route 
            //
            //   commonClass:
            //     common name of invoking object`s class
            //     One of the enumeration values that specifies the rules for the comparison.
            //
            // Возврат:
            //     Object`s name 
            //
            // Исключения:
            //   T:System.Exception:
            //     router.ActionData["className"] or commonClass is null, empty or consists only of white-space
            //     characters
            //   T:System.Exception:
            //     resulted object`s name is null, empty or consists only of white-space
            //     characters
            MatchUrlPath = false;

            if (Routes.Count > 0)
            {
                foreach (var item in Routes)
                {
                    if (item.Name.Equals(Router.Name))
                    {
                        throw new DublicateNameException();
                    }
                }

            }

            ParseTemplate(Router);

            return this;
        }

        public IWebSocketRouteHandler AddRouteData(IWebSocketRouter router)
        {
            if (MatchUrlPath)
            {
                Routes.Add(router);
                Router = router;
            }

            return this;
        }

        public void Build(IWebSocketRouteBuilder builder)
        {
            if (!MatchUrlPath)
            {
                throw new WrongUrlPathException();
            }
            else
            {
                if (builder.Context.Request.Path.Equals(Router.PathName))
                {
                    builder.ContextPathFound = true;

                    if (!builder.Context.WebSockets.IsWebSocketRequest)
                    {
                        builder.Context.Response.StatusCode = 400;
                    }
                }
            }
        }
    }
}
