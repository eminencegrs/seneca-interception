namespace Seneca.Interception.Core.Tests.Interceptors.Stubs;

public interface IStubWithGenericTask
{
    Task<ModelStub> GetModel(string id);
}