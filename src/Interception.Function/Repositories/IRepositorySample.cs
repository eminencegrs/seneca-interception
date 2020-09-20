using System.Threading.Tasks;

namespace Interception.Function.Repositories
{
    public interface IRepositorySample
    {
        Task<EntitySample> GetEntity(int id);
    }
}
