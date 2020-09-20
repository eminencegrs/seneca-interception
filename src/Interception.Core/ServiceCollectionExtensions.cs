using Castle.DynamicProxy;
using Interception.Core.Interceptors;
using Interception.Core.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Interception.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInterception(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAndRegister<InterceptorSettings>(configuration, "Interceptor");

            services.AddSingleton<IInterceptor, LoggingWithoutArgumentsInterceptor>();
            services.AddSingleton<IInterceptor, LoggingWithArgumentsInterceptor>();

            services.AddSingleton<IInterceptorSelector, InterceptorSelector>();
            services.AddSingleton<IProxyGenerationHook, LoggingProxyGeneratorHook>();

            services.AddSingleton<ProxyGenerationOptions>(provider =>
            {
                var hook = provider.GetRequiredService<IProxyGenerationHook>();
                var selector = provider.GetRequiredService<IInterceptorSelector>();
                return new ProxyGenerationOptions(hook) { Selector = selector };
            });

            services.AddSingleton<IProxyGenerator, ProxyGenerator>(provider => new ProxyGenerator());

            return services;
        }

        public static IServiceCollection AddSingletonWithInterceptors<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddSingleton<TImplementation>();
            services.AddSingleton<TInterface>(provider =>
            {
                var target = provider.GetRequiredService<TImplementation>();
                var interceptors = provider.GetServices<IInterceptor>();
                var settings = provider.GetRequiredService<InterceptorSettings>();
                var interceptorSelector = provider.GetRequiredService<IInterceptorSelector>();
                var options = provider.GetRequiredService<ProxyGenerationOptions>();
                var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();

                var proxy = proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(
                    target,
                    options,
                    interceptors.ToArray());

                return proxy;
            });

            return services;
        }

        private static IServiceCollection ConfigureAndRegister<TSettings>(this IServiceCollection services, IConfiguration configuration, string sectionName)
            where TSettings : class, new()
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            sectionName = sectionName ?? throw new ArgumentNullException(nameof(sectionName));

            services.Configure<TSettings>(configuration.GetSection(sectionName));
            services.AddSingleton<TSettings>(provider => provider.GetRequiredService<IOptions<TSettings>>().Value);

            return services;
        }
    }
}
