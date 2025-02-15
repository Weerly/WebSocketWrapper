using System;
using System.Collections.Generic;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Abstractions
{
    public interface IWebSocketRouter
    {
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
