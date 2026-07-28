using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

public class CompanyCommentConfig : IEntityTypeConfiguration<CompanyComment>
{
    public void Configure(EntityTypeBuilder<CompanyComment> builder)
    {
        builder
            .Property(x => x.Description)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .Property(x => x.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.CompanyComments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Company)
            .WithMany(x => x.CompanyComments)
            .HasForeignKey(x => x.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}