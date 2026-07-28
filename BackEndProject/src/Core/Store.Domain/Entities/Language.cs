using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Language : BaseEntity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public bool IsDefault { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsRtl { get; private set; }

    public static Language Create(
        string code,
        string name,
        bool isActive,
        bool isDefault,
        int displayOrder,
        bool isRtl)
        => new()
        {
            Code = code.Trim(),
            Name = name.Trim(),
            IsActive = isActive,
            IsDefault = isDefault,
            DisplayOrder = displayOrder,
            IsRtl = isRtl
        };

    public void Update(string code, string name, bool isActive, int displayOrder, bool isRtl)
    {
        Code = code.Trim();
        Name = name.Trim();
        IsActive = isActive;
        DisplayOrder = displayOrder;
        IsRtl = isRtl;
    }

    public void SetAsDefault() => IsDefault = true;

    public void ClearDefault() => IsDefault = false;
}
