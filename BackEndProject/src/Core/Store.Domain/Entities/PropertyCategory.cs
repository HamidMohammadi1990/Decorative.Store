using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PropertyCategory : BaseEntity
{
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;


    public ICollection<Property> Properties { get; private set; } = default!;


    public static PropertyCategory Create(string title)
        => new()
        {
            Title = title
        };

    public void Update(string title)
    {
        Title = title;
    }

    public void DeActive()
    {
        IsActive = false;
    }
}