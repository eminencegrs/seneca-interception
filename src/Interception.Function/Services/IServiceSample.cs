using System.Threading.Tasks;

namespace Interception.Function.Services
{
    public interface IServiceSample
    {
        Task<ModelSample> GetModel(int id);
    }
}