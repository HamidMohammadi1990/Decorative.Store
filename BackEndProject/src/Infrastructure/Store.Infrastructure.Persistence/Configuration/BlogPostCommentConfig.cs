using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class BlogPostCommentConfig : IEntityTypeConfiguration<BlogPostComment>
{
    public void Configure(EntityTypeBuilder<BlogPostComment> builder)
    {
        builder
            .Property(x => x.Content)
            .HasNVarcharMaxLength(2500);

        builder
            .HasOne(x => x.CreatedByUser)
            .WithMany(x => x.BlogPostComments)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ApprovedByUser)
            .WithMany(x => x.BlogPostApprovedComments)
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.BlogPost)
            .WithMany(x => x.BlogPostComments)
            .HasForeignKey(x => x.BlogPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.BlogPostId);
    }
}