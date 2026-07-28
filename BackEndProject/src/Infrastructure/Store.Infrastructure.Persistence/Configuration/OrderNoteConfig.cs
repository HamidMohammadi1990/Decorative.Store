using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class OrderNoteConfig : IEntityTypeConfiguration<OrderNote>
{
    public void Configure(EntityTypeBuilder<OrderNote> builder)
    {
        builder
            .Property(x => x.Title)
            .HasNVarcharMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasNVarcharMaxLength(300)
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.OrderNotes)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderNotes)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.OrderId);

        builder
            .HasIndex(x => x.CreatedByUserId);
    }
}