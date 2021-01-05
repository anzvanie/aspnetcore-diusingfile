using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace DependencyInjectionHelper.Test
{
    class DependencyInjectionHelperExtensionsTest
    {
        private IServiceCollection _services;
        private IServiceProvider _serviceProvider;

        [OneTimeSetUp]
        public void SetupAll()
        {
            this._services = new ServiceCollection();
            this._serviceProvider = this._services.BuildServiceProvider();
        }

        [Test]
        public void AddDependencyInjectionConfiguration_Normal1()
        {
            DependencyInjectionHelperExtensions.AddDependencyInjectionConfiguration(this._services, "Data\\di_normal_singleton.json");
        }
    }
}
