using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class PageTypeGuideConfig : IEntityTypeConfiguration<PageTypeGuide>
{
    public void Configure(EntityTypeBuilder<PageTypeGuide> builder)
    {
        builder.ToTable("PageTypeGuide");
        builder.HasKey(x => x.Type);

        builder
            .Property(x => x.AdminDescription)
            .HasNVarcharMaxLength(500)
            .IsRequired();
    }
}
