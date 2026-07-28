using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SectionItem : BaseEntity
{
    public int SectionId { get; private set; }
    public string Title { get; private set; } = default!;
    public int Priority { get; private set; }
    public string? Icon { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? Url { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;


    public Section Section { get; private set; } = default!;


    public static SectionItem Create(
        int sectionId,
        string title,
        int priority,
        string? icon,
        string? imageUrl,
        string? url,
        string? description,
        bool isActive)
        => new()
        {
            SectionId = sectionId,
            Title = title,
            Priority = priority,
            Icon = icon,
            ImageUrl = imageUrl,
            Url = url,
            Description = description,
            IsActive = isActive
        };

    public void Update(
        int sectionId,
        string title,
        int priority,
        string? icon,
        string? imageUrl,
        string? url,
        string? description,
        bool isActive)
    {
        SectionId = sectionId;
        Title = title;
        Priority = priority;
        Icon = icon;
        ImageUrl = imageUrl;
        Url = url;
        Description = description;
        IsActive = isActive;
    }
}