namespace Seneca.Interception.Samples.Function.Repositories;

public interface IRepositorySample
{
    Task<EntitySample?> GetEntity(int id);
}

