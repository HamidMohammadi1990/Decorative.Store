using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Edition.Domain.Enums;
using Edition.Application.Contracts.ContentPolicies;
using Store.Domain.ContentPolicies;

namespace Store.Infrastructure.Persistence.ContentPolicies;

public sealed class ContentPolicyEntityAccessQuery
    (EditionDbContext context, IContentEntityTypeRegistry entityTypeRegistry)
    : IContentPolicyEntityAccessQuery
{
    public Task<bool> ExistsAsync(
        string entityType,
        int resourceId,
        LambdaExpression? filter,
        CancellationToken cancellationToken = default)
    {
        var clrType = entityTypeRegistry.GetClrType(entityType);
        var idProperty = ContentPolicyPropertyReflection.GetIdProperty(clrType);

        var parameter = Expression.Parameter(clrType, "entity");
        var idEquals = Expression.Equal(
            Expression.Property(parameter, idProperty),
            Expression.Constant(resourceId));

        Expression predicate = idEquals;
        if (filter is not null)
        {
            var filterBody = new ExpressionParameterReplacer(filter.Parameters[0], parameter).Visit(filter.Body)
                ?? throw new InvalidOperationException("Unable to apply content policy filter to entity access query.");

            predicate = Expression.AndAlso(idEquals, filterBody);
        }

        var lambda = Expression.Lambda(predicate, parameter);
        var query = ContentPolicyQueryableReflection.ApplyWhere(
            ContentPolicyQueryableReflection.GetQueryable(context, clrType),
            clrType,
            lambda);
        return ContentPolicyQueryableReflection.ExecuteAnyAsync(query, clrType, cancellationToken);
    }
}

internal static class ContentPolicyPropertyReflection
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo> IdProperties = new();

    public static PropertyInfo GetIdProperty(Type entityType)
        => IdProperties.GetOrAdd(entityType, type =>
            type.GetProperty("Id")
            ?? throw new InvalidOperationException($"Entity '{type.Name}' does not have an Id property."));
}
