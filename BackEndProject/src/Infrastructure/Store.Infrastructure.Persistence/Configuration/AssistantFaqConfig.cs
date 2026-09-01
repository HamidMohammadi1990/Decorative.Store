using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

internal class AssistantFaqConfig : IEntityTypeConfiguration<AssistantFaq>
{
    public void Configure(EntityTypeBuilder<AssistantFaq> builder)
    {
        builder
            .Property(x => x.Question)
            .HasNVarcharMaxLength(200)
            .IsRequired();

        builder
            .Property(x => x.Answer)
            .HasNVarcharMaxLength(2000)
            .IsRequired();

        builder
            .HasOne(x => x.Language)
            .WithMany()
            .HasForeignKey(x => x.LanguageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.LanguageId, x.Priority });
    }
}
