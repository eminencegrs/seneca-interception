namespace Seneca.Interception.Core.Tests.Interceptors.Stubs;

public class ModelStub
{
    public string? Id { get; set; } = Guid.NewGuid().ToString();
}