using Castle.DynamicProxy;

namespace Interception.Core.Interceptors
{
    public abstract class LoggingInterceptorBase : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                this.BeforeExecution(invocation);

                invocation.Proceed();
            }
            finally
            {
                this.AfterExecution(invocation);
            }
        }

        protected abstract void BeforeExecution(IInvocation invocation);

        protected abstract void AfterExecution(IInvocation invocation);
    }
}
