using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Configuration;

public class UserStoryCommentConfig : IEntityTypeConfiguration<UserStoryComment>
{
    public void Configure(EntityTypeBuilder<UserStoryComment> builder)
    {
        builder
            .Property(x => x.Content)
            .HasNVarcharMaxLength(500);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserStoryComments)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ApprovedByUser)
            .WithMany(x => x.UserStoryApprovedComments)
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.UserStory)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.UserStoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.UserStoryId);

        builder
            .HasIndex(x => x.IsApproved);
    }
}
