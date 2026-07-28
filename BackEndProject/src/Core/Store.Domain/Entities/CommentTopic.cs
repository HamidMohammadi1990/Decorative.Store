using Store.Domain.Common;

namespace Store.Domain.Entities;

public class CommentTopic : BaseEntity
{
    public string Title { get; private set; } = default!;
    public int Priority { get; private set; }
    public bool IsActive { get; private set; } = true;


    public ICollection<ProductComment> ProductComments { get; set; } = default!;

    public static CommentTopic Create(string title, int priority)
        => new()
        {
            Title = title,
            Priority = priority
        };

    public void Update(string title, int priority, bool isActive)
    {
        Title = title;
        Priority = priority;
        IsActive = isActive;
    }
}