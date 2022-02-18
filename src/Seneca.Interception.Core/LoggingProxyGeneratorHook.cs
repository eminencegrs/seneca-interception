using Castle.DynamicProxy;
using Seneca.Interception.Core.Settings;
using System;
using System.Reflection;

namespace Seneca.Interception.Core;

public class LoggingProxyGeneratorHook : IProxyGenerationHook
{
    private readonly InterceptorSettings settings;

    public LoggingProxyGeneratorHook(InterceptorSettings settings)
    {
        this.settings = settings;
    }

    public void MethodsInspected()
    {
    }

    public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
    {
    }

    public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
    {
        return this.settings.Logging.IsEnabled;
    }
}
