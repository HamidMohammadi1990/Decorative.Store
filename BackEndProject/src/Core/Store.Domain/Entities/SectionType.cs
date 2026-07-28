using Store.Domain.Common;

namespace Store.Domain.Entities;

public class SectionType : BaseEntity
{
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;


    public ICollection<Section> Sections { get; private set; } = default!;


    public static SectionType Create(string name, bool isActive)
        => new()
        {
            Name = name,
            IsActive = isActive
        };

    public void Update(string name, bool isActive)
    {
        Name = name;
        IsActive = isActive;
    }
}