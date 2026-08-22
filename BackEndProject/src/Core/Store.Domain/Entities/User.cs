using Store.Domain.Common;

namespace Store.Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; private set; } = default!;
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public string? PhoneNumber { get; private set; }
    public bool PhoneNumberConfirmed { get; private set; }
    public bool LoginPermission { get; private set; }
    public string PasswordHash { get; private set; } = null!;
    public GenderType? Gender { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime? LastLoginDateOnUtc { get; private set; }
    public int AccessFailedCount { get; private set; }
    public RefundMethodType RefundMethod { get; private set; }
    public string SecurityStamp { get; private set; } = null!;
    public string? EconomicCode { get; private set; }


    public ICollection<Order> Orders { get; private set; } = default!;
    public ICollection<Wallet> Wallets { get; private set; } = default!;
    public ICollection<UserRole> UserRoles { get; private set; } = default!;
    public ICollection<BlogPost> BlogPosts { get; private set; } = default!;
    public ICollection<OrderNote> OrderNotes { get; private set; } = default!;
    public ICollection<UserAddress> UserAddresses { get; private set; } = default!;
    public ICollection<Discount> ProductDiscounts { get; private set; } = default!;
    public ICollection<BlogPostLike> BlogPostLikes { get; private set; } = default!;
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = default!;
    public ICollection<UserSession> UserSessions { get; private set; } = default!;
    public ICollection<ProductComment> ProductComments { get; private set; } = default!;
    public ICollection<ProductWishlist> ProductWishlists { get; private set; } = default!;
    public ICollection<UserStory> UserStories { get; private set; } = default!;
    public ICollection<BlogPostComment> BlogPostComments { get; private set; } = default!;
    public ICollection<BankTransaction> BankTransactions { get; private set; } = default!;
    public ICollection<ChequeTransaction> ChequeTransactions { get; private set; } = default!;
    public ICollection<WalletTransaction> WalletTransactions { get; private set; } = default!;
    public ICollection<BlogPostComment> BlogPostApprovedComments { get; private set; } = default!;


    public static User Create(string? email, GenderType gender, string username,
                              string firstName, string lastName, string phoneNumber,
                              string passwordHash, string securityStamp)
        => new()
        {
            Email = email,
            Gender = gender,
            UserName = username,
            LastName = lastName,
            FirstName = firstName,
            PhoneNumber = phoneNumber,
            PasswordHash = passwordHash,
            SecurityStamp = securityStamp
        };
    public static User Create(string userName, string? email, string? phoneNumber)
        => new()
        {
            Email = email,
            UserName = userName,
            PhoneNumber = phoneNumber
        };
    public User SetUserRoles(List<UserRole> userRoles)
    {
        UserRoles = userRoles;
        return this;
    }
    public void UpdateEmail(string email)
    {
        Email = email;
    }
    public void UpdateUserName(string userName)
    {
        UserName = userName;
    }
    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        SecurityStamp = Guid.NewGuid().ToString("N");
    }
    public void UpdatePhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }
    public void ConfirmPhoneNumber()
    {
        PhoneNumberConfirmed = true;
    }

    public bool EnsureSecurityStamp()
    {
        if (!string.IsNullOrWhiteSpace(SecurityStamp))
            return false;

        SecurityStamp = Guid.NewGuid().ToString("N");
        return true;
    }
}