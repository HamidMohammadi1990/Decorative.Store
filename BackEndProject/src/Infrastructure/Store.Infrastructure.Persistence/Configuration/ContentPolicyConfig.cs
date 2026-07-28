using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ContentPolicyConfig : IEntityTypeConfiguration<ContentPolicy>
{
    public void Configure(EntityTypeBuilder<ContentPolicy> builder)
    {
        builder
            .Property(x => x.Name)
            .HasNVarcharMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.EntityType)
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnType("VARCHAR(100)")
            .IsRequired();

        builder
            .Property(x => x.QueryAction)
            .HasConversion<int>()
            .HasDefaultValue(ContentPolicyQueryAction.All);

        builder
            .Property(x => x.Effect)
            .HasConversion<int>()
            .HasDefaultValue(ContentPolicyEffect.Allow);

        builder
            .Property(x => x.MergeMode)
            .HasConversion<int>()
            .HasDefaultValue(ContentPolicyMergeMode.Additive);

        builder
            .HasOne(x => x.Role)
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Rules)
            .WithOne(x => x.Policy)
            .HasForeignKey(x => x.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.RecordAccesses)
            .WithOne(x => x.Policy)
            .HasForeignKey(x => x.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => new { x.RoleId, x.EntityType, x.QueryAction, x.IsActive });

        builder
            .HasIndex(x => new { x.UserId, x.EntityType, x.QueryAction, x.IsActive });

        builder.ToTable(t => t.HasCheckConstraint(
            "CK_ContentPolicy_Scope",
            "([RoleId] IS NOT NULL AND [UserId] IS NULL) OR ([RoleId] IS NULL AND [UserId] IS NOT NULL)"));
    }
}
