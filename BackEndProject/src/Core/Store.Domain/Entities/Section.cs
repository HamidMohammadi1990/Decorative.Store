using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Section : BaseEntity
{
    public int? ParentId { get; private set; }
    public int SectionTypeId { get; private set; }
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public string Url { get; private set; } = default!;
    public string? ImageUrl { get; private set; }
    public DateTime? StartDateOnUtc { get; private set; }
    public DateTime? EndDateOnUtc { get; private set; }
    public bool IsActive { get; private set; } = true;


    public SectionType SectionType { get; private set; } = default!;
    public Section? Parent { get; private set; }
    public ICollection<Section> Children { get; private set; } = [];
    public ICollection<SectionItem> SectionItems { get; private set; } = [];
    public ICollection<PageSection> PageSections { get; private set; } = [];


    public static Section Create(
        int sectionTypeId,
        int? parentId,
        string title,
        string? description,
        string url,
        string? imageUrl,
        DateTime? startDateOnUtc,
        DateTime? endDateOnUtc,
        bool isActive)
        => new()
        {
            SectionTypeId = sectionTypeId,
            ParentId = parentId,
            Title = title,
            Description = description,
            Url = url,
            ImageUrl = imageUrl,
            StartDateOnUtc = startDateOnUtc,
            EndDateOnUtc = endDateOnUtc,
            IsActive = isActive
        };

    public void Update(
        int sectionTypeId,
        int? parentId,
        string title,
        string? description,
        string url,
        string? imageUrl,
        DateTime? startDateOnUtc,
        DateTime? endDateOnUtc,
        bool isActive)
    {
        SectionTypeId = sectionTypeId;
        ParentId = parentId;
        Title = title;
        Description = description;
        Url = url;
        ImageUrl = imageUrl;
        StartDateOnUtc = startDateOnUtc;
        EndDateOnUtc = endDateOnUtc;
        IsActive = isActive;
    }

    public bool IsVisibleAt(DateTime utcNow, bool sectionTypeIsActive)
        => IsActive
           && sectionTypeIsActive
           && (!StartDateOnUtc.HasValue || StartDateOnUtc <= utcNow)
           && (!EndDateOnUtc.HasValue || EndDateOnUtc >= utcNow);
}