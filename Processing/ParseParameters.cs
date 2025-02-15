using static Weerly.WebSocketWrapper.WebSocketEnums;

namespace Weerly.WebSocketWrapper.Processing
{
    /// <summary>
    /// Provides methods for retrieving configured parameters based on the parameter type.
    /// </summary>
    public static class ParseParameters
    {
        /// <summary>
        /// Retrieves an array of configured parameters based on the specified parameter type.
        /// </summary>
        /// <param name="param">The type of parameter to retrieve.</param>
        /// <returns>An array of strings representing the configured parameters.</returns>
        public static string[] GetConfiguredParams(ParamsType param)
        {
            var configuredParams = new string[0];

            if (param == ParamsType.Type)
            {
                configuredParams = new [] {
                    "Controller",
                    "Class",
                    "Controller",
                    "Controller",
                    "Controller"
                };
            }

            if (param == ParamsType.Template)
            {
                configuredParams = new [] {
                    "{controller}/{action}",
                    "{class}/{action}",
                    "/{action}",
                    "{}/{}",
                    "/{}"
                };
            }

            if (param == ParamsType.Patterns)
            {
                configuredParams = new []
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