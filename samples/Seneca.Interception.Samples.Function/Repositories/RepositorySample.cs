using Microsoft.Extensions.Logging;

namespace Seneca.Interception.Samples.Function.Repositories;

public class RepositorySample : IRepositorySample
{
    private static readonly IReadOnlyList<EntitySample> entities = new List<EntitySample>
    {
        new EntitySample { Id = 1, Description = $"{nameof(EntitySample)}1" },
        new EntitySample { Id = 2, Description = $"{nameof(EntitySample)}2" },
        new EntitySample { Id = 3, Description = $"{nameof(EntitySample)}3" }
    };

    private readonly ILogger<RepositorySample> logger;

    public RepositorySample(ILogger<RepositorySample> logger)
    {
        this.logger = logger;
    }

    public Task<EntitySample?> GetEntity(int id)
    {
        this.logger.LogInformation($"{nameof(RepositorySample)} is going to return an entity.");

        return Task.FromResult(entities.FirstOrDefault(entity => entity.Id == id));
    }
}
