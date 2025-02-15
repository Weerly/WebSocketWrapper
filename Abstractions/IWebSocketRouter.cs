using System;
using System.Collections.Generic;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Abstractions
{
    /// <summary>
    /// Defines the structure for a WebSocket router, providing properties and methods
    /// for managing WebSocket routes, templates, namespaces, patterns, and types.
    /// </summary>
    public interface IWebSocketRouter
    {
        bool IsAsync { get; set; }
        string Name { get; }
        string Template { get; }
        CommonType Type { get; set; }
        string PathName { get; set; }
        string ClassNamespace { get; }
        IDictionary<string, string> ActionData { get; set; }
        string[] Types { get; }
        string[] Patterns { get; }

        string GetNamespace(string rootNamespace, string classNamespace);


    }
}
