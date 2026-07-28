using Store.Domain.Common;

namespace Store.Domain.Entities;

public class Role : BaseEntity
{
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public bool BypassContentPolicy { get; private set; }
    public bool RequireContentPolicy { get; private set; }


    public ICollection<RolePermission> RolePermissions { get; private set; } = default!;
    public ICollection<UserRole> UserRoles { get; private set; } = default!;


    public static Role Create(string title)
        => new()
        {
            Title = title
        };

    public void Update(string title, bool isActive)
    {
        Title = title;
        IsActive = isActive;
    }

    public void SetBypassContentPolicy(bool bypassContentPolicy)
        => BypassContentPolicy = bypassContentPolicy;

    public void SetRequireContentPolicy(bool requireContentPolicy)
        => RequireContentPolicy = requireContentPolicy;
}