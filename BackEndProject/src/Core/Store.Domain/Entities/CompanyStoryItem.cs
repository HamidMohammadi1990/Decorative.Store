using Store.Domain.Common;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class CompanyStoryItem : BaseEntity
{
    public int CompanyStoryId { get; private set; }
    public CompanyStoryMediaType MediaType { get; private set; }
    public string FileName { get; private set; } = default!;
    public int Priority { get; private set; }
    public int? DurationSeconds { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;


    public CompanyStory CompanyStory { get; private set; } = default!;


    public static CompanyStoryItem Create(
        int companyStoryId,
        CompanyStoryMediaType mediaType,
        string fileName,
        int priority,
        int? durationSeconds = null)
        => new()
        {
            FileName = fileName,
            Priority = priority,
            MediaType = mediaType,
            CompanyStoryId = companyStoryId,
            DurationSeconds = durationSeconds
        };

    public void Update(
        CompanyStoryMediaType mediaType,
        string fileName,
        int priority,
        int? durationSeconds = null)
    {
        FileName = fileName;
        Priority = priority;
        MediaType = mediaType;
        DurationSeconds = durationSeconds;
    }
}
