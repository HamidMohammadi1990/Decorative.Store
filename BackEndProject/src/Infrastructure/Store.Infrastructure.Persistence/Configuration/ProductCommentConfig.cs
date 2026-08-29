using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProductCommentConfig : IEntityTypeConfiguration<ProductComment>
{
    public void Configure(EntityTypeBuilder<ProductComment> builder)
    {
        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(250)
            .IsRequired();

        builder
            .HasOne(d => d.CommentTopic)
            .WithMany(p => p.ProductComments)
            .HasForeignKey(x => x.CommentTopicId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.Product)
            .WithMany(p => p.ProductComments)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.User)
            .WithMany(p => p.ProductComments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(d => d.Parent)
            .WithMany(p => p.Replies)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.ProductId);

        builder
            .HasIndex(x => x.UserId);
    }
}