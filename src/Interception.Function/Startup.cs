using Interception.Core;
using Interception.Function;
using Interception.Function.Repositories;
using Interception.Function.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Interception.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(context.ApplicationRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile("local.settings.json", true, true)
                .Build();

            builder.Services.AddLogging();
            builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddInterception(configuration);

            builder.Services.AddSingletonWithInterceptors<IServiceSample, ServiceSample>();
            builder.Services.AddSingletonWithInterceptors<IRepositorySample, RepositorySample>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Converters = { new StringEnumConverter() } };
        }
    }
}