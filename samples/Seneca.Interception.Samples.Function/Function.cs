using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Seneca.Interception.Samples.Function.Services;

namespace Seneca.Interception.Samples.Function;

public class Function
{
    private readonly ILogger<Function> logger;
    private readonly IServiceSample service;

    public Function(ILogger<Function> logger, IServiceSample service)
    {
        this.logger = logger;
        this.service = service;
    }

    [Function("get-model-by-id")]
    public async Task<HttpResponseData> Get(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "models/{id}")]
        HttpRequestData httpRequestData,
        FunctionContext executionContext,
        int id)
    {
        var model = await this.service.GetModel(id);

        throw new NotImplementedException();
    }
}
