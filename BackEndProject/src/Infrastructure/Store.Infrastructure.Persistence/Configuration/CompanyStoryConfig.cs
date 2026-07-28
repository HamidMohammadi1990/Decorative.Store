using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class CompanyStoryConfig : IEntityTypeConfiguration<CompanyStory>
{
    public void Configure(EntityTypeBuilder<CompanyStory> builder)
    {
        builder
            .Property(x => x.Caption)
            .HasNVarcharMaxLength(500);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.CompanyStories)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.CreatedByUser)
            .WithMany(x => x.CreatedCompanyStories)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Items)
            .WithOne(x => x.CompanyStory)
            .HasForeignKey(x => x.CompanyStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Likes)
            .WithOne(x => x.CompanyStory)
            .HasForeignKey(x => x.CompanyStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Comments)
            .WithOne(x => x.CompanyStory)
            .HasForeignKey(x => x.CompanyStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.CompanyId);

        builder
            .HasIndex(x => x.CreatedByUserId);

        builder
            .HasIndex(x => x.IsActive);
    }
}
