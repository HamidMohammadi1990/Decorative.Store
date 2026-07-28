using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Edition.Application.Models.ContentPolicies;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Services.ContentPolicies;

internal static class ContentPolicyPropertyReflectionCache
{
    private static readonly ConcurrentDictionary<(Type Type, string Name), PropertyInfo?> Properties = new();

    public static PropertyInfo? GetProperty(Type type, string name)
        => Properties.GetOrAdd(
            (type, name),
            static key => key.Type.GetProperty(
                key.Name,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase));
}

internal static class ContentPolicyExpressionBuildInvoker
{
    private static readonly ConcurrentDictionary<Type, Func<ContentPolicyExpressionBuilder, string, object, ContentPolicyContext, LambdaExpression?>> Builders = new();

    public static LambdaExpression? Build(
        ContentPolicyExpressionBuilder builder,
        IContentEntityTypeRegistry entityTypeRegistry,
        string entityTypeName,
        object policies,
        ContentPolicyContext context)
    {
        var entityClrType = entityTypeRegistry.GetClrType(entityTypeName);
        var typedBuilder = Builders.GetOrAdd(entityClrType, CreateBuilder);
        return typedBuilder(builder, entityTypeName, policies, context);
    }

    private static Func<ContentPolicyExpressionBuilder, string, object, ContentPolicyContext, LambdaExpression?> CreateBuilder(Type entityClrType)
    {
        var method = typeof(ContentPolicyExpressionBuilder)
            .GetMethod(nameof(ContentPolicyExpressionBuilder.Build))!
            .MakeGenericMethod(entityClrType);

        return (builder, entityTypeName, policies, context) =>
            method.Invoke(builder, [entityTypeName, policies, context]) as LambdaExpression;
    }
}

public sealed class ContentPolicyCompiledFilterCache
{
    private readonly ConcurrentDictionary<string, LambdaExpression> _cache = new();

    public LambdaExpression? GetOrAdd(string cacheKey, Func<LambdaExpression?> factory)
    {
        if (_cache.TryGetValue(cacheKey, out var cached))
            return cached;

        var built = factory();
        if (built is null)
            return null;

        return _cache.GetOrAdd(cacheKey, built);
    }

    public void Clear()
        => _cache.Clear();
}
