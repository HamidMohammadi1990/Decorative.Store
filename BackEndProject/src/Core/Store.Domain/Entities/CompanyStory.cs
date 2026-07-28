using Store.Domain.Common;

namespace Store.Domain.Entities;

public class CompanyStory : BaseEntity
{
    public int CompanyId { get; private set; }
    public int CreatedByUserId { get; private set; }
    public string? Caption { get; private set; }
    public DateTime CreatedOnUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; private set; }
    public DateTime? ExpiresAtUtc { get; private set; }
    public bool IsActive { get; private set; } = true;


    public Company Company { get; private set; } = default!;
    public User CreatedByUser { get; private set; } = default!;
    public ICollection<CompanyStoryItem> Items { get; private set; } = [];
    public ICollection<CompanyStoryLike> Likes { get; private set; } = [];
    public ICollection<CompanyStoryComment> Comments { get; private set; } = [];


    public static CompanyStory Create(
        int companyId,
        int createdByUserId,
        string? caption = null,
        DateTime? expiresAtUtc = null)
        => new()
        {
            Caption = caption,
            CompanyId = companyId,
            ExpiresAtUtc = expiresAtUtc,
            CreatedByUserId = createdByUserId
        };

    public void Update(string? caption, DateTime? expiresAtUtc)
    {
        Caption = caption;
        ExpiresAtUtc = expiresAtUtc;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void AddItems(params CompanyStoryItem[] items)
    {
        foreach (var item in items)
            Items.Add(item);
    }

    public void ReplaceItems(IEnumerable<CompanyStoryItem> items)
    {
        Items.Clear();
        foreach (var item in items)
            Items.Add(item);
    }
}
