using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class UserStory : BaseEntity
{
    public int UserId { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Caption { get; private set; }
    public StoryMediaType MediaType { get; private set; }
    public string MediaPath { get; private set; } = default!;
    public string MediaAlt { get; private set; } = default!;
    public string? PosterPath { get; private set; }
    public int? ProductId { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;

    public User User { get; private set; } = default!;
    public Product? Product { get; private set; }
    public ICollection<UserStoryComment> Comments { get; private set; } = [];
    public ICollection<UserStoryLike> Likes { get; private set; } = [];

    public static UserStory Create(
        int userId,
        string title,
        string? caption,
        StoryMediaType mediaType,
        string mediaPath,
        string mediaAlt,
        string? posterPath,
        int? productId)
        => new()
        {
            UserId = userId,
            Title = title,
            Caption = caption,
            MediaType = mediaType,
            MediaPath = mediaPath,
            MediaAlt = mediaAlt,
            PosterPath = posterPath,
            ProductId = productId,
            IsActive = true,
        };

    public void Update(string title, string? caption, bool isActive, int? productId)
    {
        Title = title;
        Caption = caption;
        IsActive = isActive;
        ProductId = productId;
    }

    public void UpdateMedia(
        StoryMediaType mediaType,
        string mediaPath,
        string mediaAlt,
        string? posterPath)
    {
        MediaType = mediaType;
        MediaPath = mediaPath;
        MediaAlt = mediaAlt;
        PosterPath = posterPath;
    }

    public void SetActive(bool isActive) => IsActive = isActive;
}
