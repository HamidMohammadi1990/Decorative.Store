using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Section : BaseEntity
{
    public int? ParentId { get; private set; }
    public int SectionTypeId { get; private set; }
    public string? ImageUrl { get; private set; }
    public DateTime? StartDateOnUtc { get; private set; }
    public DateTime? EndDateOnUtc { get; private set; }
    public bool IsActive { get; private set; } = true;

    public SectionType SectionType { get; private set; } = default!;
    public Section? Parent { get; private set; }
    public ICollection<Section> Children { get; private set; } = [];
    public ICollection<SectionItem> SectionItems { get; private set; } = [];
    public ICollection<PageSection> PageSections { get; private set; } = [];
    public ICollection<SectionTranslation> Translations { get; private set; } = [];

    public static Section Create(
        int sectionTypeId,
        int? parentId,
        string? imageUrl,
        DateTime? startDateOnUtc,
        DateTime? endDateOnUtc,
        bool isActive)
        => new()
        {
            SectionTypeId = sectionTypeId,
            ParentId = parentId,
            ImageUrl = imageUrl,
            StartDateOnUtc = startDateOnUtc,
            EndDateOnUtc = endDateOnUtc,
            IsActive = isActive,
        };

    public SectionTranslation UpsertTranslation(int languageId, string title, string url, string? description = null)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title, url, description);
            return existing;
        }

        var translation = SectionTranslation.Create(title, url, languageId, description);
        Translations.Add(translation);
        return translation;
    }

    public void Update(
        int sectionTypeId,
        int? parentId,
        string? imageUrl,
        DateTime? startDateOnUtc,
        DateTime? endDateOnUtc,
        bool isActive,
        int languageId,
        string title,
        string url,
        string? description)
    {
        SectionTypeId = sectionTypeId;
        ParentId = parentId;
        ImageUrl = imageUrl;
        StartDateOnUtc = startDateOnUtc;
        EndDateOnUtc = endDateOnUtc;
        IsActive = isActive;
        UpsertTranslation(languageId, title, url, description);
    }

    public bool IsVisibleAt(DateTime utcNow, bool sectionTypeIsActive)
        => IsActive
           && sectionTypeIsActive
           && (!StartDateOnUtc.HasValue || StartDateOnUtc <= utcNow)
           && (!EndDateOnUtc.HasValue || EndDateOnUtc >= utcNow);
}
