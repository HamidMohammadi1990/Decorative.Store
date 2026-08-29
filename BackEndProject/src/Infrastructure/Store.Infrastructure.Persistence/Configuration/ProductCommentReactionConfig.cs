using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductCommentReactionConfig : IEntityTypeConfiguration<ProductCommentReaction>
{
    public void Configure(EntityTypeBuilder<ProductCommentReaction> builder)
    {
        builder
            .HasOne(x => x.ProductComment)
            .WithMany(x => x.Reactions)
            .HasForeignKey(x => x.ProductCommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.UserId, x.ProductCommentId })
            .IsUnique();
    }
}
