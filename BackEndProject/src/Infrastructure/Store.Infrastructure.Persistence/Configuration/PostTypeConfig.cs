using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class PostTypeConfig : IEntityTypeConfiguration<PostType>
{
    public void Configure(EntityTypeBuilder<PostType> builder)
    {
        builder
           .Property(x => x.Title)
           .HasNVarcharMaxLength(35)
           .IsRequired();

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(150)
            .IsRequired();

        builder
            .HasMany(x => x.OrderItems)
            .WithOne(x => x.PostType)
            .HasForeignKey(x => x.PostTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}