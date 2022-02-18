using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Seneca.Interception.Core;
using Seneca.Interception.Samples.Function.Repositories;
using Seneca.Interception.Samples.Function.Services;

namespace Seneca.Interception.Samples.Function.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder RegisterDependencies(this IHostBuilder hostBuilder)
    {
        if (hostBuilder == null)
        {
            throw new ArgumentNullException(nameof(hostBuilder));
        }

        hostBuilder.ConfigureServices(services =>
        {
            services.AddLogging();

            services.AddHttpClient();
        });

        hostBuilder.ConfigureServices(services =>
        {
            services.AddInterception();

            services.AddSingletonWithInterceptors<IServiceSample, ServiceSample>();
            
            services.AddSingletonWithInterceptors<IRepositorySample, RepositorySample>();
        });

        return hostBuilder;
    }
}
