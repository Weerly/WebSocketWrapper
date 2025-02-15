using System;
using System.Collections.Generic;
using Weerly.WebSocketWrapper.Abstractions;
using Weerly.WebSocketWrapper.Processing;
using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Routing
{

    public class WebSocketRouter : IWebSocketRouter
    {
        public string Name { get; private set; }
        public string Template { get; private set; }
        public string PathName { get; set; }
        public IDictionary<string, string> ActionData { get; set; }
        public string[] Types { get; private set; }
        public string[] Patterns { get; private set; }
        public CommonType Type { get; set; }
        public string ClassNamespace { get; }

        public WebSocketRouter(string name, string template)
        {
            ApplyCommonProperties(name, template, CommonType.Controller);
        }
        public WebSocketRouter(string name, string template, CommonType type)
        {
            ApplyCommonProperties(name, template, type);
        }
        public WebSocketRouter(string name, string template, CommonType type, string classNamespace)
        {
            ClassNamespace = classNamespace;
            ApplyCommonProperties(name, template, type);
        }

        public string GetNamespace(string rootNamespace, string classNamespace)
        {
            if (string.IsNullOrEmpty(classNamespace))
            {
                throw new Exception("class namespace should not be null");
            }

            Console.WriteLine(rootNamespace);
            Console.WriteLine(classNamespace);

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

        private void ApplyCommonProperties(string name, string template, CommonType type)
        {
            Name = name;
            Template = template;
            Type = type;
            Patterns = ParseParameters.GetConfiguredParams(ParamsType.Patterns);
            Types = ParseParameters.GetConfiguredParams(ParamsType.Type);
            ActionData = new Dictionary<string, string>();
        }
    }
}
