namespace Seneca.Interception.Samples.Function.Services;

public interface IServiceSample
{
    Task<ModelSample> GetModel(int id);
}
