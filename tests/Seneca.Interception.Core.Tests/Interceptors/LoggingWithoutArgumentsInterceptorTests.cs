using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Xunit;

namespace Seneca.Interception.Core.Tests.Interceptors;

public class LoggingWithoutArgumentsInterceptorTests
{
    private readonly LoggingWithoutArgumentsInterceptorFixture fixture = new();

    [Fact]
    public void ShouldNotThrowAnyException_WhenCallIntercept_GivenInvocation()
    {
        var (sut, invocation) = this.fixture.GetEnvironment();

        Action act = () => sut.Intercept(invocation.Object);

        using (new AssertionScope())
        {
            act.Should().NotThrow();

            invocation.Verify(i => i.Proceed(), Times.Once);
            invocation.Verify(i => i.InvocationTarget, Times.Exactly(2));
            invocation.Verify(i => i.Method, Times.Exactly(3));
            invocation.Verify(i => i.Arguments, Times.Once);
            invocation.Verify(i => i.ReturnValue, Times.Once);
        }
    }

    [Fact]
    public void ShouldNotThrowAnyException_WhenCallIntercept_GivenInvocation2()
    {
        var (sut, invocation) = this.fixture.GetEnvironment2();

        Action act = () => sut.Intercept(invocation.Object);

        using (new AssertionScope())
        {
            act.Should().NotThrow();

            invocation.Verify(i => i.Proceed(), Times.Once);
            invocation.Verify(i => i.InvocationTarget, Times.Exactly(2));
            invocation.Verify(i => i.Method, Times.Exactly(3));
            invocation.Verify(i => i.Arguments, Times.Once);
            invocation.Verify(i => i.ReturnValue, Times.Never);
        }
    }

    [Fact]
    public void ShouldNotThrowAnyException_WhenCallIntercept_GivenInvocation3()
    {
        var (sut, invocation) = this.fixture.GetEnvironment3();

        Action act = () => sut.Intercept(invocation.Object);

        using (new AssertionScope())
        {
            act.Should().NotThrow();

            invocation.Verify(i => i.Proceed(), Times.Once);
            invocation.Verify(i => i.InvocationTarget, Times.Exactly(2));
            invocation.Verify(i => i.Method, Times.Exactly(3));
            invocation.Verify(i => i.Arguments, Times.Once);
            invocation.Verify(i => i.ReturnValue, Times.Once);
        }
    }

    [Fact]
    public void ShouldVerifyInternalCalls_WhenCallIntercept_GivenInvocationWithException()
    {
        var (sut, invocation) = this.fixture.GetEnvironment(isValid: false);

        var action = () => sut.Intercept(invocation.Object);

        using (new AssertionScope())
        {
            action.Should().ThrowExactly<InvalidOperationException>();

            invocation.Verify(i => i.Proceed(), Times.Once);
            invocation.Verify(i => i.Method, Times.Exactly(3));
            invocation.Verify(i => i.Arguments, Times.Once);
            invocation.Verify(i => i.ReturnValue, Times.Once);
        }
    }
}
