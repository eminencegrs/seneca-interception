using Castle.DynamicProxy;
using Interception.Core.Settings;
using System;
using System.Reflection;

namespace Interception.Core
{
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
            if (!this.settings.Logging.IsEnabled)
            {
                return false;
            }

            return true;
        }
    }
}
