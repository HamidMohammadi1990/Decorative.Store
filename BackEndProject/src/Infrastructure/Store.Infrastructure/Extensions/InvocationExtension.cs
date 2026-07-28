using Castle.DynamicProxy;
using Store.Domain.Enums;

namespace Store.Infrastructure.Extensions;

public static class InvocationExtension
{
    public static MethodType GetDelegateType(this IInvocation invocation)
    {
        var returnType = invocation.Method.ReturnType;
        if (returnType == typeof(Task))
            return MethodType.AsyncAction;
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            return MethodType.AsyncFunction;
        return MethodType.Synchronous;
    }
    public static string GetMethodKey(this IInvocation invocation)
    {
        var key = invocation.Arguments.Length > 0 ? $"-{invocation.Arguments[0]}" : "";
        var cacheKey = $"{invocation.TargetType!.FullName?.Replace('.', ':') ?? ""}" +
                       $".{invocation.Method.Name}" +
                       $"{key}";
        return cacheKey;
    }
    public static CancellationToken GetCancellationToken(this IInvocation invocation)
    {
        var parameters = invocation.Method.GetParameters();
        for (int i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].ParameterType == typeof(CancellationToken))
                return (CancellationToken)invocation.Arguments[i]!;
        }

        return CancellationToken.None;
    }

    public static async Task<TResult> ProceedAsync<TResult>(this IInvocation invocation)
    {
        ArgumentNullException.ThrowIfNull(invocation);
        
        var methodInfo = invocation.MethodInvocationTarget!;
        var result = methodInfo.Invoke(invocation.InvocationTarget, invocation.Arguments);

        if (result is Task<TResult> taskT)        
            return await taskT.ConfigureAwait(false);        

        if (result is Task task)
        {
            await task.ConfigureAwait(false);
            return default!;
        }

        throw new InvalidOperationException(
            $"Method {methodInfo.Name} does not return Task or Task<{typeof(TResult).Name}>");
    }
}