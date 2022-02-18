using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Castle.DynamicProxy;
using Seneca.Interception.Core.Interceptors;
using Seneca.Interception.Core.Settings;

namespace Seneca.Interception.Core;

public class InterceptorSelector : IInterceptorSelector
{
    private readonly ILogger<InterceptorSelector> logger;
    private readonly InterceptorSettings settings;

    public InterceptorSelector(InterceptorSettings settings, ILogger<InterceptorSelector> logger)
    {
        this.logger = logger;
        this.settings = settings;
    }

    public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
    {
        var registeredInterceptors = string.Join("; ", interceptors.Select(i => i.GetType().Name));
        this.logger.LogTrace($"Selecting interceptors from registered ones: '{{registeredInterceptors}}'",
            registeredInterceptors);

        var filteredInterceptors = settings.Logging.CanTrackArguments
            ? interceptors.Where(i => !(i is LoggingWithoutArgumentsInterceptor)).ToArray()
            : interceptors.Where(i => !(i is LoggingWithParametersInterceptor)).ToArray();

        var selectedInterceptors = string.Join("; ", filteredInterceptors.Select(i => i.GetType().Name));
        this.logger.LogTrace($"Selected interceptors: '{{interceptors}}'", selectedInterceptors);

        return filteredInterceptors;
    }
}
