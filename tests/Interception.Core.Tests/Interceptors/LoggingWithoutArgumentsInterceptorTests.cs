using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Xunit;

namespace Interception.Core.Tests.Interceptors
{
    public class LoggingWithoutArgumentsInterceptorTests
    {
        private readonly LoggingWithoutArgumentsInterceptorFixture fixture = new LoggingWithoutArgumentsInterceptorFixture();

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

        //[Fact]
        //public void ShouldVerifyInternalCalls_WhenCallIntercept_GivenInvocationWithException()
        //{
        //    var (logger, invocation, processor, factory, context) = this.fixture.GetEnvironment(isExceptionalCase: true);

        //    var sut = new LoggingInterceptor(processor.Object, factory.Object, logger);

        //    Action act = () => sut.Intercept(invocation.Object);

        //    using (new AssertionScope())
        //    {
        //        act.Should().ThrowExactly<InvalidOperationException>();

        //        invocation.Verify(i => i.Proceed(), Times.Once);
        //        invocation.Verify(i => i.Method, Times.Exactly(2));
        //        invocation.Verify(i => i.Arguments, Times.Once);
        //        invocation.Verify(i => i.ReturnValue, Times.Once);

        //        factory.Verify(f => f.CreateInterceptionContext(It.IsAny<Castle.DynamicProxy.IInvocation>()), Times.Once);

        //        processor.Verify(p => p.AfterExecution(It.IsAny<IInterceptionContext>(), It.IsAny<ILogger<LoggingInterceptor>>()), Times.Once);
        //        processor.Verify(p => p.BeforeExecution(It.IsAny<IInterceptionContext>(), It.IsAny<ILogger<LoggingInterceptor>>()), Times.Once);
        //    }
        //}
    }
}
