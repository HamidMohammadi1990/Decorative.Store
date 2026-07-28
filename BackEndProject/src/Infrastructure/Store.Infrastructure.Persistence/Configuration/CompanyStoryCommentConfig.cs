using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class CompanyStoryCommentConfig : IEntityTypeConfiguration<CompanyStoryComment>
{
    public void Configure(EntityTypeBuilder<CompanyStoryComment> builder)
    {
        builder
            .Property(x => x.Content)
            .HasNVarcharMaxLength(2500);

        builder
            .HasOne(x => x.CreatedByUser)
            .WithMany(x => x.CompanyStoryComments)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ApprovedByUser)
            .WithMany(x => x.CompanyStoryApprovedComments)
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.CompanyStory)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.CompanyStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.CompanyStoryId);
    }
}
