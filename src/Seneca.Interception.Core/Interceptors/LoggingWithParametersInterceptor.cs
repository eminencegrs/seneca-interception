using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Castle.DynamicProxy;
using Seneca.Interception.Core.Settings;

namespace Seneca.Interception.Core.Interceptors;

public class LoggingWithParametersInterceptor : LoggingInterceptorBase
{
    private readonly ILogger<LoggingWithParametersInterceptor> logger;
    private readonly LoggingSettings loggingSettings;

    public LoggingWithParametersInterceptor(
        ILogger<LoggingWithParametersInterceptor> logger,
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
            var methodName = invocation.Method.Name;

            if (invocation.Arguments.Length > 0)
            {
                var parameters = GetSerializedParameters(invocation.Arguments);

                this.logger.Log(
                    this.loggingSettings.LogLevel,
                    $"Executing '{{targetMethod}}' [{{targetClass}}]. Parameters: '{{parameters}}'.",
                    methodName,
                    className,
                    parameters);
            }
            else
            {
                this.logger.Log(
                    this.loggingSettings.LogLevel,
                    $"Executing '{{targetMethod}}' [{{targetClass}}] without parameters.",
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
            var methodName = invocation.Method.Name;

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
                var task = (Task) invocation.ReturnValue;
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
                var task = (Task) invocation.ReturnValue;
                task.ContinueWith(previous =>
                {
                    var result = previous.GetType().GetProperty("Result")?.GetValue(previous, null);
                    if (result == null)
                    {
                        return;
                    }

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

    private string GetSerializedParameters(object[] parameters)
    {
        try
        {
            this.logger.LogTrace($"Serializing parameters...");

            var serialized = JsonSerializer.Serialize(parameters);

            this.logger.LogTrace($"Parameters have been successfully serialized.");

            return serialized;
        }
        catch (Exception ex)
        {
            this.logger.LogTrace(ex, $"Could not serialize parameters.");

            return "Could not serialize parameters.";
        }
    }

    private string GetReturnValueForLogging(object result)
    {
        try
        {
            this.logger.LogTrace($"Serializing a return value...");

            var returnValue = JsonSerializer.Serialize(result);

            this.logger.LogTrace($"Return value has been successfully serialized.");

            return returnValue;
        }
        catch (Exception ex)
        {
            this.logger.LogTrace(ex, $"Could not serialize a return value.");

            return "Could not serialize a return value.";
        }
    }
}
