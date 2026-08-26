using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SectionTypeTranslation : BaseEntity
{
    public int SectionTypeId { get; private set; }
    public int LanguageId { get; private set; }
    public string Name { get; private set; } = default!;

    public SectionType SectionType { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static SectionTypeTranslation Create(string name, int languageId)
        => new()
        {
            Name = name,
            LanguageId = languageId,
        };

    public void Update(string name)
    {
        Name = name;
    }
}
