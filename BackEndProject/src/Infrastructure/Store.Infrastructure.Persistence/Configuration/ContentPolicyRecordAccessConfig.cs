using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ContentPolicyRecordAccessConfig : IEntityTypeConfiguration<ContentPolicyRecordAccess>
{
    public void Configure(EntityTypeBuilder<ContentPolicyRecordAccess> builder)
    {
        builder
            .HasOne(x => x.Policy)
            .WithMany(x => x.RecordAccesses)
            .HasForeignKey(x => x.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.PolicyId);

        builder
            .HasIndex(x => new { x.PolicyId, x.EntityId })
            .IsUnique();
    }
}
