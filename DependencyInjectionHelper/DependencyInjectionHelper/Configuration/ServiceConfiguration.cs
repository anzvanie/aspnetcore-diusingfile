using Newtonsoft.Json;

namespace DependencyInjectionHelper.Configuration
{
    [JsonObject]
    internal class ServiceConfiguration
    {
        /// <summary>
        /// Name of target configuration service
        /// </summary>
        [JsonProperty("ServiceName")]
        public string ServiceName { get; set; }

        /// <summary>
        /// Name of implementation class which implements target service
        /// </summary>
        [JsonProperty("ImplementationName")]
        public string ImplementationName { get; set; }

        /// <summary>
        /// Setting scope in context
        /// </summary>
        [JsonProperty("Scope")]
        public string Scope { get; set; } = null;
    }
}
