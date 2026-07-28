using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class CommentTopicConfig : IEntityTypeConfiguration<CommentTopic>
{
    public void Configure(EntityTypeBuilder<CommentTopic> builder)
    {
        builder
           .Property(x => x.Title)
           .HasNVarcharMaxLength(35)
           .IsRequired();

        builder
            .HasMany(x => x.ProductComments)
            .WithOne(x => x.CommentTopic)
            .HasForeignKey(x => x.CommentTopicId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}