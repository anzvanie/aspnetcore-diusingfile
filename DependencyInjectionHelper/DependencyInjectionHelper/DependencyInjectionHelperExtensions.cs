using DependencyInjectionHelper.Configuration;
using DependencyInjectionHelper.Exception;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DependencyInjectionHelper
{
    /// <summary>
    /// Extension class for <see cref="IServiceCollection"/>
    /// </summary>
    public static class DependencyInjectionHelperExtensions
    {
        /// <summary>
        /// Add DI configuration using a json configuration file
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configurationFile">Json configuration file, default is "di.json"</param>
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services, string configurationFile = "di.json")
        {
            var serviceConfigurations = DependencyInjectionHelperExtensions._GetServiceConfigurationFromFile(configurationFile);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var configuration in serviceConfigurations.ToList())
                {
                    var serviceType = asm.GetType(configuration.ServiceName);
                    var implementationType = asm.GetType(configuration.ImplementationName);

                    serviceConfigurations.Remove(configuration);
                }
            }


        }

        /// <summary>
        /// Read configuration from file and storing in a list of <see cref="ServiceConfiguration"/>
        /// </summary>
        /// <param name="configurationFile">Json configuration file</param>
        /// <returns>List of configuration</returns>
        private static IList<ServiceConfiguration> _GetServiceConfigurationFromFile(string configurationFile)
        {
            using (var fs = File.OpenText(configurationFile))
            {
                var serializer = new JsonSerializer();
                var serviceConfigurations = serializer.Deserialize(fs, typeof(List<ServiceConfiguration>));
                return (List<ServiceConfiguration>)serviceConfigurations;
            }
        }

        /// <summary>
        /// Add dependency injection for each configuration
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="ServiceConfiguration"/></param>
        private static void _ConfigureService(IServiceCollection services, Type serviceType, Type implementationType, string scope)
        {
            if (serviceType == null)
            {
                throw new InvalidTypeException();
            }

            if (implementationType == null)
            {
                throw new InvalidTypeException();
            }

            switch (scope)
            {
                case ScopeSettings.Scoped:
                    services.AddScoped(serviceType, implementationType);
                    break;
                case ScopeSettings.Transient:
                    services.AddTransient(serviceType, implementationType);
                    break;
                case ScopeSettings.Singleton:
                case ScopeSettings.NotSpecified:
                    services.AddSingleton(serviceType, implementationType);
                    break;
                default:
                    throw new InvalidScopeException();
            }
        }
    }

    /// <summary>
    /// Setting value of service scope
    /// </summary>
    static class ScopeSettings
    {
        /// <summary>
        /// Specifying to call <c>IServiceCollection.AddScoped</c>
        /// </summary>
        public const string Scoped = "Scoped";

        /// <summary>
        /// Specifying to call <c>IServiceCollection.AddTransient</c>
        /// </summary>
        public const string Transient = "Transient";

        /// <summary>
        /// Specifying to call <c>IServiceCollection.AddSingleton</c>
        /// </summary>
        public const string Singleton = "Singleton";

        /// <summary>
        /// When Scope is not specified, default scope is "Singleton"
        /// </summary>
        public const string NotSpecified = null;
    }
}
