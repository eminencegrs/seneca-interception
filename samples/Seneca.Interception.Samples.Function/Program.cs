using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Seneca.Interception.Samples.Function.Extensions;

namespace Seneca.Interception.Samples.Function;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            // https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#configuration
            //.ConfigureFunctionsWorkerDefaults(builder => builder.UseMiddleware<ErrorHandlingMiddleware>())
            .ConfigureAppConfiguration(
                config => config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("local.settings.json", optional: true)
                    .AddEnvironmentVariables())
            .RegisterDependencies()
            .Build();

        await host.RunAsync();
    }
}
