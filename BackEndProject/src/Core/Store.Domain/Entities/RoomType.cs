using Store.Domain.Common;

namespace Store.Domain.Entities;

public class RoomType : BaseEntity
{
    public string Code { get; private set; } = default!;
    public string ImageFileName { get; private set; } = default!;
    public int Priority { get; private set; }
    public bool IsActive { get; private set; } = true;

    public ICollection<RoomTypeTranslation> Translations { get; private set; } = [];

    public static RoomType Create(string code, string imageFileName, int priority)
        => new()
        {
            Code = code,
            ImageFileName = imageFileName,
            Priority = priority,
        };

    public RoomTypeTranslation UpsertTranslation(int languageId, string title)
    {
        var existing = Translations.FirstOrDefault(x => x.LanguageId == languageId);
        if (existing is not null)
        {
            existing.Update(title);
            return existing;
        }

        var translation = RoomTypeTranslation.Create(title, languageId);
        Translations.Add(translation);
        return translation;
    }

    public void Update(string code, string imageFileName, int priority, bool isActive, int languageId, string title)
    {
        Code = code;
        ImageFileName = imageFileName;
        Priority = priority;
        IsActive = isActive;
        UpsertTranslation(languageId, title);
    }
}
