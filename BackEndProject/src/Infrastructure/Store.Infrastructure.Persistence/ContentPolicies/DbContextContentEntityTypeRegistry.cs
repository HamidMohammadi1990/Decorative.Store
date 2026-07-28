using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Store.Domain.ContentPolicies;

namespace Store.Infrastructure.Persistence.ContentPolicies;

public sealed class DbContextContentEntityTypeRegistry : IContentEntityTypeRegistry
{
    private static readonly Lazy<IReadOnlyDictionary<string, Type>> EntityTypes = new(DiscoverEntityTypes);

    public bool IsRegistered(string entityTypeName)
        => EntityTypes.Value.ContainsKey(entityTypeName);

    public Type GetClrType(string entityTypeName)
        => EntityTypes.Value.TryGetValue(entityTypeName, out var clrType)
            ? clrType
            : throw new NotSupportedException($"Content entity type '{entityTypeName}' is not registered.");

    public string GetEntityPrefix(string entityTypeName)
        => entityTypeName;

    public IReadOnlyList<string> GetRegisteredNamesOrderedByLengthDesc()
        => [.. EntityTypes.Value.Keys.OrderByDescending(x => x.Length)];

    public IReadOnlyList<string> GetRegisteredEntityTypeNames()
        => [.. EntityTypes.Value.Keys.OrderBy(x => x, StringComparer.Ordinal)];

    private static IReadOnlyDictionary<string, Type> DiscoverEntityTypes()
    {
        var result = new Dictionary<string, Type>(StringComparer.Ordinal);

        foreach (var property in typeof(EditionDbContext).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.PropertyType.IsGenericType
                || property.PropertyType.GetGenericTypeDefinition() != typeof(DbSet<>))
                continue;

            var clrType = property.PropertyType.GetGenericArguments()[0];
            if (ShouldExclude(clrType))
                continue;

            if (!typeof(Domain.Common.IEntity<int>).IsAssignableFrom(clrType))
                continue;

            result[clrType.Name] = clrType;
        }

        return result;
    }

    private static bool ShouldExclude(Type clrType)
    {
        if (clrType == typeof(Domain.Entities.ContentPolicy) || clrType == typeof(Domain.Entities.ContentPolicyRule))
            return true;

        return clrType.GetCustomAttribute<ExcludeFromContentPolicyAttribute>() is not null;
    }
}
