using Castle.DynamicProxy;
using Interception.Core.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Interception.Core.Interceptors
{
    public class LoggingWithArgumentsInterceptor : LoggingInterceptorBase
    {
        private readonly ILogger<LoggingWithArgumentsInterceptor> logger;
        private readonly LoggingSettings loggingSettings;

        public LoggingWithArgumentsInterceptor(
            ILogger<LoggingWithArgumentsInterceptor> logger,
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
                    var arguments = GetSerializedArguments(invocation.Arguments);

                    this.logger.Log(
                        this.loggingSettings.LogLevel,
                        $"Executing '{{targetMethod}}' [{{targetClass}}]. Arguments: '{{arguments}}'.",
                        methodName,
                        className,
                        arguments);
                }
                else
                {
                    this.logger.Log(
                        this.loggingSettings.LogLevel,
                        $"Executing '{{targetMethod}}' [{{targetClass}}]. There are no arguments.",
                        methodName,
                        className);
                }
            }
            catch
            {
                // Do not break the normal program flow, just skip it.
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
                        $"Executed '{{targetMethod}}' [{{targetClass}}]. Return type: '{{returnType}}'.",
                        methodName,
                        className,
                        returnType);
                }

                if (returnType == typeof(Task))
                {
                    var task = (Task)invocation.ReturnValue;
                    task.ContinueWith((antecedent) =>
                    {
                        this.logger.Log(
                            this.loggingSettings.LogLevel,
                            $"Executed '{{targetMethod}}' [{{targetClass}}]. Return type: '{{returnType}}'.",
                            methodName,
                            className,
                            returnType);
                    }).ConfigureAwait(false);

                    task.Wait();
                }
                else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var task = (Task)invocation.ReturnValue;
                    task.ContinueWith((antecedent) =>
                    {
                        var result = antecedent.GetType().GetProperty("Result").GetValue(antecedent, null);
                        var returnValue = this.GetReturnValueForLogging(result);
                        logger.Log(
                            this.loggingSettings.LogLevel,
                            $"Executed '{{targetMethod}}' [{{targetClass}}]. Return value: '{{returnValue}}'.",
                            methodName,
                            className,
                            returnValue);
                    }).ConfigureAwait(false);

                    task.Wait();
                }
                else
                {
                    var returnValue = this.GetReturnValueForLogging(invocation.ReturnValue);
                    logger.Log(
                        this.loggingSettings.LogLevel,
                        $"Executed '{{targetMethod}}' [{{targetClass}}]. Return value: '{{returnValue}}'.",
                        methodName,
                        className,
                        returnValue);
                }
            }
            catch
            {
                // Do not break the normal program flow, just skip it.
            }
        }

        private string GetSerializedArguments(object[] arguments)
        {
            try
            {
                this.logger.LogTrace($"Serializing arguments...");

                var serialized = JsonConvert.SerializeObject(arguments);

                this.logger.LogTrace($"Arguments have been successfully serialized.");

                return serialized;
            }
            catch (Exception ex)
            {
                this.logger.LogTrace(ex, $"Could not serialize arguments.");

                return "Could not serialize arguments.";
            }
        }

        private string GetReturnValueForLogging(object result)
        {
            try
            {
                this.logger.LogTrace($"Serializing return value...");

                var returnValue = JsonConvert.SerializeObject(result);

                this.logger.LogTrace($"Return value has been successfully serialized.");

                return returnValue;
            }
            catch (Exception ex)
            {
                this.logger.LogTrace(ex, $"Could not serialize return value.");

                return "Could not serialize return value.";
            }
        }
    }
}
