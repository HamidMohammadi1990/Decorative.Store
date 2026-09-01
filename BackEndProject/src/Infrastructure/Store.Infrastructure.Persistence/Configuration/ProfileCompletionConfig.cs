using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configuration;

internal class ProfileCompletionSettingConfig : IEntityTypeConfiguration<ProfileCompletionSetting>
{
    public void Configure(EntityTypeBuilder<ProfileCompletionSetting> builder)
    {
        builder
            .Property(x => x.ConfigJson)
            .HasColumnType("NVARCHAR(MAX)")
            .IsRequired();
    }
}

internal class ProfileCompletionUserStateConfig : IEntityTypeConfiguration<ProfileCompletionUserState>
{
    public void Configure(EntityTypeBuilder<ProfileCompletionUserState> builder)
    {
        builder
            .Property(x => x.AnswersJson)
            .HasColumnType("NVARCHAR(MAX)")
            .IsRequired();

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(x => x.UserId)
            .IsUnique();
    }
}
