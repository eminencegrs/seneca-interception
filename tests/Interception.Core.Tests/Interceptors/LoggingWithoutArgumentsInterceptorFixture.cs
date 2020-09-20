using System;
using System.Threading.Tasks;
using AutoFixture;
using Interception.Core.Interceptors;
using Interception.Core.Settings;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Interception.Core.Tests.Interceptors
{
    public class LoggingWithoutArgumentsInterceptorFixture
    {
        private readonly IFixture fixture = new Fixture();

        public (
            LoggingWithoutArgumentsInterceptor sut,
            Mock<Castle.DynamicProxy.IInvocation> invocation)
            GetEnvironment(bool isValid = true)
        {
            var id = Guid.NewGuid().ToString();
            var model = this.fixture.Build<ModelStub>().With(m => m.Id, id).Create();

            var stub = new Mock<IStubWithGenericTask>();
            stub.Setup(s => s.GetModel(It.IsAny<string>())).Returns(Task.FromResult(model));

            var invocation = new Mock<Castle.DynamicProxy.IInvocation>();
            invocation.SetupGet(i => i.InvocationTarget).Returns(stub.Object);
            invocation.SetupGet(i => i.Method).Returns(typeof(IStubWithGenericTask).GetMethod(nameof(IStubWithGenericTask.GetModel))).Verifiable();
            invocation.SetupGet(i => i.Arguments).Returns(new[] { id }).Verifiable();
            invocation.SetupGet(i => i.ReturnValue).Returns(Task.FromResult(model)).Verifiable();

            if (isValid)
            {
                invocation.Setup(i => i.Proceed()).Verifiable();
            }
            else
            {
                invocation.Setup(i => i.Proceed()).Throws<InvalidOperationException>().Verifiable();
            }

            var logger = NullLogger<LoggingWithoutArgumentsInterceptor>.Instance;

            var settings = this.fixture.Create<InterceptorSettings>();

            var interceptor = new LoggingWithoutArgumentsInterceptor(logger, settings);

            return (interceptor, invocation);
        }

        public (
            LoggingWithoutArgumentsInterceptor sut,
            Mock<Castle.DynamicProxy.IInvocation> invocation)
            GetEnvironment2(bool isValid = true)
        {
            var id = this.fixture.Create<int>();

            var stub = new Mock<IStubWithVoid>();
            stub.Setup(s => s.Handle(It.IsAny<int>()));

            var invocation = new Mock<Castle.DynamicProxy.IInvocation>();
            invocation.SetupGet(i => i.InvocationTarget).Returns(stub.Object);
            invocation.SetupGet(i => i.Method).Returns(typeof(IStubWithVoid).GetMethod(nameof(IStubWithVoid.Handle))).Verifiable();
            invocation.SetupGet(i => i.Arguments).Returns(new object[] { id }).Verifiable();
            invocation.SetupGet(i => i.ReturnValue).Returns(typeof(void)).Verifiable();

            if (isValid)
            {
                invocation.Setup(i => i.Proceed()).Verifiable();
            }
            else
            {
                invocation.Setup(i => i.Proceed()).Throws<InvalidOperationException>().Verifiable();
            }

            var logger = NullLogger<LoggingWithoutArgumentsInterceptor>.Instance;

            var settings = this.fixture.Create<InterceptorSettings>();

            var interceptor = new LoggingWithoutArgumentsInterceptor(logger, settings);

            return (interceptor, invocation);
        }

        public (
            LoggingWithoutArgumentsInterceptor sut,
            Mock<Castle.DynamicProxy.IInvocation> invocation)
            GetEnvironment3(bool isValid = true)
        {
            var stub = new Mock<IStubWithTask>();
            stub.Setup(s => s.Run()).Returns(Task.CompletedTask);

            var invocation = new Mock<Castle.DynamicProxy.IInvocation>();
            invocation.SetupGet(i => i.InvocationTarget).Returns(stub.Object);
            invocation.SetupGet(i => i.Method).Returns(typeof(IStubWithTask).GetMethod(nameof(IStubWithTask.Run))).Verifiable();
            invocation.SetupGet(i => i.Arguments).Returns(new object[] {}).Verifiable();
            invocation.SetupGet(i => i.ReturnValue).Returns(Task.CompletedTask).Verifiable();

            if (isValid)
            {
                invocation.Setup(i => i.Proceed()).Verifiable();
            }
            else
            {
                invocation.Setup(i => i.Proceed()).Throws<InvalidOperationException>().Verifiable();
            }

            var logger = NullLogger<LoggingWithoutArgumentsInterceptor>.Instance;

            var settings = this.fixture.Create<InterceptorSettings>();

            var interceptor = new LoggingWithoutArgumentsInterceptor(logger, settings);

            return (interceptor, invocation);
        }
    }

    public class ModelStub
    {
        public string Id { get; set; }
    }

    public interface IStubWithGenericTask
    {
        Task<ModelStub> GetModel(string id);
    }

    public interface IStubWithVoid
    {
        void Handle(int id);
    }

    public interface IStubWithTask
    {
        Task Run();
    }
}
