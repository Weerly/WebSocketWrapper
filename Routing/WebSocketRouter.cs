using System;
using System.Collections.Generic;
using Weerly.WebSocketWrapper.Abstractions;
using Weerly.WebSocketWrapper.Processing;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Routing
{
    /// <summary>
    /// Represents a WebSocket router responsible for managing WebSocket routes,
    /// including their names, templates, and associated properties.
    /// </summary>
    public class WebSocketRouter : IWebSocketRouter
    {
        /// <summary>
        /// Specifies whether the WebSocket router operates asynchronously.
        /// </summary>
        /// <remarks>
        /// This property determines if the WebSocket routing logic will execute asynchronously.
        /// Setting this property to true enables asynchronous operation, facilitating non-blocking
        /// task execution for performance optimization in scenarios involving high concurrency.
        /// </remarks>
        public bool IsAsync { get; set; }

        /// Gets the name of the WebSocket route that uniquely identifies this router.
        /// This property represents the identifying name assigned during initialization of a WebSocketRouter
        /// instance, which ensures distinction between various routes. It is immutable after the router is created.
        public string Name { get; private set; }

        /// Represents the template associated with the WebSocket route.
        /// This property defines the route's URL structure or matching pattern.
        /// It is used to determine how incoming WebSocket requests are routed
        /// based on the specified template configuration.
        public string Template { get; private set; }

        /// <summary>
        /// Gets or sets the path name associated with the WebSocket route.
        /// </summary>
        /// <remarks>
        /// This property is used to define or retrieve the specific path associated with the route,
        /// allowing for flexible routing within WebSocket-based applications.
        /// </remarks>
        public string PathName { get; set; }

        /// <summary>
        /// Represents a collection of actions associated with a WebSocket route.
        /// </summary>
        /// <remarks>
        /// Key-value pairs in this dictionary define specific actions and their corresponding data
        /// for the associated WebSocket route. This property can be set and accessed to manage
        /// actions specific to route definitions.
        /// </remarks>
        public IDictionary<string, string> ActionData { get; set; }

        /// <summary>
        /// Gets an array of strings representing the configured types for the WebSocket router.
        /// </summary>
        /// <remarks>
        /// This property is initialized using the <c>ParseParameters.GetConfiguredParams</c> method with the <c>ParamsType.Type</c> parameter.
        /// The types represent a collection of specific identifiers or classifications associated with the router.
        /// </remarks>
        public string[] Types { get; private set; }

        /// <summary>
        /// Gets the array of patterns associated with the WebSocket router.
        /// These patterns define specific configurations or routing parameters
        /// for processing WebSocket requests.
        /// </summary>
        public string[] Patterns { get; private set; }

        /// Represents the common type of a WebSocket route in the router.
        /// This property is used to specify whether the route is associated with a class or controller.
        public CommonType Type { get; set; }

        /// <summary>
        /// Gets the namespace associated with the class to which the WebSocket router belongs.
        /// This property is used to identify the class context for routing purposes.
        /// </summary>
        public string ClassNamespace { get; }

        /// <summary>
        /// Represents a router specifically designed for WebSocket-based applications.
        /// Provides routing capabilities including namespace handling, template configuration,
        /// and parameter parsing for WebSocket routes.
        /// </summary>
        public WebSocketRouter(string name, string template)
        {
            ApplyCommonProperties(name, template, CommonType.Controller);
        }

        /// Represents a WebSocket route configuration for routing WebSocket messages to appropriate handlers.
        public WebSocketRouter(string name, string template, CommonType type)
        {
            ApplyCommonProperties(name, template, type);
        }

        /// <summary>
        /// Represents a WebSocketRouter that encapsulates routing logic for WebSocket communication,
        /// allowing for configuration and namespace management.
        /// </summary>
        public WebSocketRouter(string name, string template, CommonType type, string classNamespace)
        {
            ClassNamespace = classNamespace;
            ApplyCommonProperties(name, template, type);
        }

        /// <summary>
        /// Represents a router for handling WebSocket routes.
        /// </summary>
        public WebSocketRouter(string name, string template, CommonType type, string classNamespace, bool isAsync)
        {
            ClassNamespace = classNamespace;
            ApplyCommonProperties(name, template, type, isAsync);
        }

        /// <summary>
        /// Retrieves the full namespace for a class by combining the root namespace and the class namespace.
        /// </summary>
        /// <param name="rootNamespace">The root namespace of the application.</param>
        /// <param name="classNamespace">The namespace of the class to be resolved.</param>
        /// <returns>The fully resolved namespace as a string.</returns>
        /// <exception cref="System.Exception">Thrown when the classNamespace parameter is null or empty.</exception>
        public string GetNamespace(string rootNamespace, string classNamespace)
        {
            if (string.IsNullOrEmpty(classNamespace))
            {
                throw new Exception("class namespace should not be null");
            }

            var index = rootNamespace.IndexOf(classNamespace, StringComparison.Ordinal);
            string resultNamespace;

            if (index != -1)
            {
                resultNamespace = classNamespace;
            }
            else
            {
                resultNamespace = $"{rootNamespace}.{classNamespace}";
            }

            return resultNamespace;
        }

        /// <summary>
        /// Applies common properties to initialize a WebSocket router.
        /// </summary>
        /// <param name="name">The name of the WebSocket router.</param>
        /// <param name="template">The template used for routing.</param>
        /// <param name="type">The type of the WebSocket router, indicating whether it is a class or controller.</param>
        /// <param name="isAsync">Indicates whether the WebSocket router operates asynchronously. Default is false.</param>
        private void ApplyCommonProperties(string name, string template, CommonType type, bool isAsync = false)
        {
            Name = name;
            Template = template;
            Type = type;
            IsAsync = isAsync;
            Patterns = ParseParameters.GetConfiguredParams(ParamsType.Patterns);
            Types = ParseParameters.GetConfiguredParams(ParamsType.Type);
            ActionData = new Dictionary<string, string>();
        }
    }
}
