using Store.Domain.Common;

namespace Store.Domain.Entities;

public class RoomTypeTranslation : BaseEntity
{
    public int RoomTypeId { get; private set; }
    public int LanguageId { get; private set; }
    public string Title { get; private set; } = default!;

    public RoomType RoomType { get; private set; } = null!;
    public Language Language { get; private set; } = null!;

    public static RoomTypeTranslation Create(string title, int languageId)
        => new()
        {
            Title = title,
            LanguageId = languageId,
        };

    public void Update(string title) => Title = title;
}
