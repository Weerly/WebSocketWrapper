using System;
using System.Collections.Generic;
using static Weerly.WebSocketWrapper.Enums.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Routing
{
    public interface IWebSocketRouter
    {
        String Name { get; }
        String Template { get; }
        CommonType Type { get; set; }
        String PathName { get; set; }
        String ClassNamespace { get; }
        IDictionary<System.String, System.String> ActionData { get; set; }
        String[] Types { get; }
        String[] Patterns { get; }

        String GetNamespace(string rootNamespace, string classNamespace);


    }
}
