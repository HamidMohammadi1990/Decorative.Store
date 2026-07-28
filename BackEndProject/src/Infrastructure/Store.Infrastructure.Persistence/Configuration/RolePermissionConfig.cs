using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class RolePermissionConfig : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder
            .HasOne(p => p.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(p => p.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(p => p.Role)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.RoleId);

        builder
            .HasIndex(x => x.PermissionId);
    }
}