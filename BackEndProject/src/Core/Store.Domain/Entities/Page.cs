using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Page : BaseEntity
{
    public string Slug { get; private set; } = default!;
    public string Title { get; private set; } = default!;
    public string? MetaTitle { get; private set; }
    public string? MetaDescription { get; private set; }
    public PageType Type { get; private set; }
    public bool IsActive { get; private set; } = true;


    public ICollection<PageSection> PageSections { get; private set; } = [];


    public static Page Create(
        string slug,
        string title,
        PageType type,
        bool isActive,
        string? metaTitle = null,
        string? metaDescription = null)
        => new()
        {
            Slug = slug.Trim(),
            Title = title,
            Type = type,
            IsActive = isActive,
            MetaTitle = metaTitle,
            MetaDescription = metaDescription
        };

    public void Update(
        string slug,
        string title,
        PageType type,
        bool isActive,
        string? metaTitle = null,
        string? metaDescription = null)
    {
        Slug = slug.Trim();
        Title = title;
        Type = type;
        IsActive = isActive;
        MetaTitle = metaTitle;
        MetaDescription = metaDescription;
    }
}