using Castle.DynamicProxy;
using Interception.Core.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Interception.Core.Interceptors
{
    public class LoggingWithoutArgumentsInterceptor : LoggingInterceptorBase
    {
        private readonly ILogger<LoggingWithoutArgumentsInterceptor> logger;
        private readonly LoggingSettings loggingSettings;

        public LoggingWithoutArgumentsInterceptor(
            ILogger<LoggingWithoutArgumentsInterceptor> logger,
            InterceptorSettings interceptorSettings)
            : base()
        {
            this.logger = logger;
            this.loggingSettings = interceptorSettings.Logging;
        }

        protected override void BeforeExecution(IInvocation invocation)
        {
            try
            {
                var className = invocation.InvocationTarget.ToString();
                var methodName = invocation.Method.Name.ToString();

                if (invocation.Arguments.Length > 0)
                {
                    this.logger.Log(
                        this.loggingSettings.LogLevel,
                        $"Executing '{{targetMethod}}' ({{targetClass}}). Arguments are not being tracked.",
                        methodName,
                        className);
                }
                else
                {
                    this.logger.Log(
                        this.loggingSettings.LogLevel,
                        $"Executing '{{targetMethod}}' ({{targetClass}}). There are no arguments.",
                        methodName,
                        className);
                }
            }
            catch
            {
                // Do not break the normal program flow.
            }
        }

        protected override void AfterExecution(IInvocation invocation)
        {
            try
            {
                var className = invocation.InvocationTarget.ToString();
                var methodName = invocation.Method.Name.ToString();

                var returnType = invocation.Method.ReturnType;
                if (returnType == typeof(void))
                {
                    this.logger.Log(
                        this.loggingSettings.LogLevel,
                        $"Executed '{{targetMethod}}' ({{targetClass}}). Return type: '{{returnType}}'.",
                        methodName,
                        className,
                        returnType);
                    return;
                }

                if (returnType == typeof(Task) || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)))
                {
                    var task = (Task)invocation.ReturnValue;
                    task.ContinueWith((antecedent) => this.Log(methodName, className, returnType)).ConfigureAwait(false);
                    task.Wait();
                }
                else
                {
                    this.Log(methodName, className, returnType);
                }
            }
            catch
            {
                // Do not break the normal program flow.
            }
        }

        private void Log(string methodName, string className, Type returnType)
        {
            this.logger.Log(
                this.loggingSettings.LogLevel,
                $"Executed '{{targetMethod}}' ({{targetClass}}). Return type: '{{returnType}}'. Return value is not being tracked.",
                methodName,
                className,
                returnType);
        }
    }
}
