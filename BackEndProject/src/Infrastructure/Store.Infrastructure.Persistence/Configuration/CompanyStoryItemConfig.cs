using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class CompanyStoryItemConfig : IEntityTypeConfiguration<CompanyStoryItem>
{
    public void Configure(EntityTypeBuilder<CompanyStoryItem> builder)
    {
        builder
            .Property(x => x.FileName)
            .HasVarcharMaxLength(150)
            .IsRequired();

        builder
            .HasOne(x => x.CompanyStory)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CompanyStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.CompanyStoryId);
    }
}
