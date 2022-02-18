using Microsoft.Extensions.Logging;
using Seneca.Interception.Samples.Function.Repositories;

namespace Seneca.Interception.Samples.Function.Services;

public class ServiceSample : IServiceSample
{
    private readonly ILogger<ServiceSample> logger;

    private readonly IRepositorySample repository;

    public ServiceSample(ILogger<ServiceSample> logger, IRepositorySample repository)
    {
        this.logger = logger;
        this.repository = repository;
    }

    public async Task<ModelSample> GetModel(int id)
    {
        var entity = await this.repository.GetEntity(id);

        this.logger.LogInformation("Mapping entity to model...");

        var model = entity != null
            ? new ModelSample { Id = entity.Id, Description = $"{nameof(ModelSample)}-{entity.Description}" }
            : null;

        this.logger.LogInformation($"{nameof(ServiceSample)} is going to return a model.");

        return model;
    }
}
