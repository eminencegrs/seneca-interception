using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Castle.DynamicProxy;
using Seneca.Interception.Core.Interceptors;
using Seneca.Interception.Core.Settings;

namespace Seneca.Interception.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterception(this IServiceCollection services)
    {
        services.AddSingleton(RegisterSettings<InterceptorSettings>("Interceptor"));

        services.AddSingleton<IInterceptor, LoggingWithoutArgumentsInterceptor>();
        services.AddSingleton<IInterceptor, LoggingWithParametersInterceptor>();

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

    public static IServiceCollection AddSingletonWithInterceptors<TInterface, TImplementation>(
        this IServiceCollection services)
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

    private static Func<IServiceProvider, TSettings> RegisterSettings<TSettings>(string sectionName)
        where TSettings : notnull
    {
        sectionName = string.IsNullOrWhiteSpace(sectionName)
            ? throw new ArgumentNullException(nameof(sectionName))
            : sectionName;

        return provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();

            var settings = configuration.GetSection(sectionName).Get<TSettings>();

            return settings;
        };
    }
}
