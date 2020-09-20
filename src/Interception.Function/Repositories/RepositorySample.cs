using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Interception.Function.Repositories
{
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

        public async Task<EntitySample> GetEntity(int id)
        {
            // It pretends to do some work here.
            await Task.Delay(5000);

            this.logger.LogInformation($"{nameof(RepositorySample)} is going to return an entity.");

            return entities.FirstOrDefault(e => e.Id == id);
        }
    }
}
