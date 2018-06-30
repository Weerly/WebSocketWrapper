using System;
using System.Collections.Generic;
using Weerly.WebSocketWrapper.Parameters;
using static Weerly.WebSocketWrapper.Enums.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Routing
{

    public class WebSocketRouter : IWebSocketRouter
    {
        public string Name { get; }
        public string Template { get; }
        public String PathName { get; set; }
        public IDictionary<String, String> ActionData { get; set; }
        public String[] Types { get; }
        public String[] Patterns { get; }
        public CommonType Type { get; set; }
        public string ClassNamespace { get; }

        public WebSocketRouter(string name, string template)
        {
            Name = name;
            Template = template;
            Type = CommonType.Controller;
            Patterns = ParseParameters.GetConfiguredParams(ParamsType.Patterns);
            Types = ParseParameters.GetConfiguredParams(ParamsType.Type);
            ActionData = new Dictionary<String, String>();
        }
        public WebSocketRouter(string name, string template, CommonType type)
        {
            Name = name;
            Template = template;
            Type = type;
            Patterns = ParseParameters.GetConfiguredParams(ParamsType.Patterns);
            Types = ParseParameters.GetConfiguredParams(ParamsType.Type);
            ActionData = new Dictionary<System.String, System.String>();
        }
        public WebSocketRouter(string name, string template, CommonType type, string classNamespace)
        {
            Name = name;
            Template = template;
            Type = type;
            ClassNamespace = classNamespace;
            Patterns = ParseParameters.GetConfiguredParams(ParamsType.Patterns);
            Types = ParseParameters.GetConfiguredParams(ParamsType.Type);
            ActionData = new Dictionary<System.String, System.String>();
        }

        public String GetNamespace(string rootNamespace, string classNamespace)
        {
            if (string.IsNullOrEmpty(classNamespace))
            {
                throw new Exception("class namespace should not be null");
            }

            int index = rootNamespace.IndexOf(classNamespace);
            System.String Namespace;

            if (index != -1)
            {
                Namespace = classNamespace;
            }
            else
            {
                Namespace = rootNamespace + "." + classNamespace;
            }

            return Namespace;
        }
    }
}
