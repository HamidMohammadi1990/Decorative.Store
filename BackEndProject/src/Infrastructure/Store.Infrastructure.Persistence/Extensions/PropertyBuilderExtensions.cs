using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Infrastructure.Persistence.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder HasVarcharMaxLength(this PropertyBuilder builder, int length)
    {
        return builder.HasColumnType($"VARCHAR({length})");
    }

    public static PropertyBuilder HasNVarcharMaxLength(this PropertyBuilder builder, int length)
    {
        return builder.HasColumnType($"NVARCHAR({length})");
    }
}