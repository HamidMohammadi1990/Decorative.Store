using Store.Domain.Common;

namespace Store.Domain.Entities;

public class PostType : BaseEntity
{
    public string Title { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int Priority { get; private set; }


    public ICollection<OrderItem> OrderItems { get; private set; } = default!;


    public static PostType Create(string title, int priority, string description)
        => new()
        {
            Title = title,
            Priority = priority,
            Description = description
        };

    public void Update(string title, string? description, bool isActive, int priority)
    {
        Title = title;
        IsActive = isActive;
        Priority = priority;
        Description = description;
    }
}