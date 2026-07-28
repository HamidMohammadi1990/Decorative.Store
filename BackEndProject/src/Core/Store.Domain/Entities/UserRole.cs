using Store.Domain.Common;

namespace Store.Domain.Entities;

public class UserRole : BaseEntity
{
    public int UserId { get; private set; }
    public int RoleId { get; private set; }


    public Role Role { get; private set; } = default!;
    public User User { get; private set; } = default!;


    public static UserRole Create(int roleId)
        => new()
        {
            RoleId = roleId
        };

    public static UserRole Create(int userId, int roleId)
        => new()
        {
            UserId = userId,
            RoleId = roleId
        };
}