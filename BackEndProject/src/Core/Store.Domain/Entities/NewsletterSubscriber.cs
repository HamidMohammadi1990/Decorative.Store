using Store.Domain.Common;

namespace Store.Domain.Entities;

public class NewsletterSubscriber : BaseEntity
{
    public string Email { get; private set; } = default!;
    public int LanguageId { get; private set; }
    public DateTime SubscribedAtUtc { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; } = true;

    public Language Language { get; private set; } = default!;

    public static NewsletterSubscriber Create(int languageId, string email)
        => new()
        {
            LanguageId = languageId,
            Email = email,
            SubscribedAtUtc = DateTime.UtcNow,
            IsActive = true,
        };

    public void Resubscribe(int languageId)
    {
        LanguageId = languageId;
        SubscribedAtUtc = DateTime.UtcNow;
        IsActive = true;
    }
}
