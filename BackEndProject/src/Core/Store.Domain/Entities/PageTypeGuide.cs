using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class PageTypeGuide
{
    public PageType Type { get; private set; }
    public string AdminDescription { get; private set; } = default!;

    public static PageTypeGuide Create(PageType type, string adminDescription)
        => new()
        {
            Type = type,
            AdminDescription = adminDescription.Trim(),
        };

    public void UpdateDescription(string adminDescription)
        => AdminDescription = adminDescription.Trim();
}
