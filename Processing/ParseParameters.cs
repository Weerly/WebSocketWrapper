using System;
using static Weerly.WebSocketWrapper.Enums.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Parameters
{
    public class ParseParameters
    {
        public static String[] GetConfiguredParams(ParamsType param)
        {
            String[] configuredParams = new string[0];

            if (param == ParamsType.Type)
            {
                configuredParams = new String[5] {
                    "Controller",
                    "Class",
                    "Controller",
                    "Controller",
                    "Controller"
                };
            }

            if (param == ParamsType.Template)
            {
                configuredParams = new String[5] {
                    "{controller}/{action}",
                    "{class}/{action}",
                    "/{action}",
                    "{}/{}",
                    "/{}"
                };
            }

            if (param == ParamsType.Patterns)
            {
                configuredParams = new string[5]
                {
                    @"^\{controller=([a-zA-Z]+)\}\/\{action=([a-zA-Z]+)\}$",
                    @"^{class=([a-zA-Z]+)\}\/\{action=([a-zA-Z]+)\}$",
                    @"^\/\{action=([a-zA-Z]+)\}$",
                    @"^([a-zA-Z]+)\/([a-zA-Z]+)$",
                    @"^\/([a-zA-Z]+)$"
                };
            }

            return configuredParams;
        }
    }
}