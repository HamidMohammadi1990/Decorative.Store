using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class RoomTypeConfig : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("RoomType");

        builder.Property(x => x.Code).HasVarcharMaxLength(32).IsRequired();
        builder.Property(x => x.ImageFileName).HasNVarcharMaxLength(120).IsRequired();

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.Priority);
    }
}

internal class RoomTypeTranslationConfig : IEntityTypeConfiguration<RoomTypeTranslation>
{
    public void Configure(EntityTypeBuilder<RoomTypeTranslation> builder)
    {
        builder.ToTable("RoomTypeTranslation");

        builder.Property(x => x.Title).HasNVarcharMaxLength(100).IsRequired();

        builder
            .HasOne(x => x.RoomType)
            .WithMany(x => x.Translations)
            .HasForeignKey(x => x.RoomTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.RoomTypeId, x.LanguageId }).IsUnique();
    }
}
