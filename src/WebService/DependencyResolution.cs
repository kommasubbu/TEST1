// Copyright (c) Microsoft. All rights reserved.

using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Diagnostics;
using Microsoft.Azure.IoTSolutions.StorageAdapter.Services.Runtime;
using Microsoft.Azure.IoTSolutions.StorageAdapter.WebService.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Azure.IoTSolutions.StorageAdapter.WebService
{
    public class DependencyResolution
    {
        /// <summary>
        /// Autofac configuration. Find more information here:
        /// @see http://docs.autofac.org/en/latest/integration/aspnetcore.html
        /// </summary>
        public static IContainer Setup(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            AutowireAssemblies(builder);
            SetupCustomRules(builder);

            var container = builder.Build();
            RegisterFactory(container);

            return container;
        }

        /// <summary>Autowire interfaces to classes from all the assemblies</summary>
        private static void AutowireAssemblies(ContainerBuilder builder)
        {
            var assembly = Assembly.GetEntryAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();

            // Auto-wire additional assemblies
            assembly = typeof(IServicesConfig).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();
        }

        /// <summary>Setup Custom rules overriding autowired ones.</summary>
        private static void SetupCustomRules(ContainerBuilder builder)
        {
            // Instantiate only one logger
            // TODO: read log level from configuration
            var logger = new Logger(Uptime.ProcessId, LogLevel.Debug);
            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();

            // Make sure the configuration is read only once.
            var config = new Config(new ConfigData(logger));
            builder.RegisterInstance(config).As<IConfig>().SingleInstance();

            // Service configuration is generated by the entry point, so we
            // prepare the instance here.
            builder.RegisterInstance(config.ServicesConfig).As<IServicesConfig>().SingleInstance();

            // By default Autofac uses a request lifetime, creating new objects
            // for each request, which is good to reduce the risk of memory
            // leaks, but not so good for the overall performance.
          
        }

        private static void RegisterFactory(IContainer container)
        {
            Factory.RegisterContainer(container);
        }

        /// <summary>
        /// Provide factory pattern for dependencies that are instantiated
        /// multiple times during the application lifetime.
        /// How to use:
        /// <code>
        /// class MyClass : IMyClass {
        ///     public MyClass(DependencyInjection.IFactory factory) {
        ///         this.factory = factory;
        ///     }
        ///     public SomeMethod() {
        ///         var instance1 = this.factory.Resolve<ISomething>();
        ///         var instance2 = this.factory.Resolve<ISomething>();
        ///         var instance3 = this.factory.Resolve<ISomething>();
        ///     }
        /// }
        /// </code>
        /// </summary>
        public interface IFactory
        {
            T Resolve<T>();
        }

        public class Factory : IFactory
        {
            private static IContainer container;

            public static void RegisterContainer(IContainer c)
            {
                container = c;
            }

            public T Resolve<T>()
            {
                return container.Resolve<T>();
            }
        }
    }
}
