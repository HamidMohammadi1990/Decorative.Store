using Store.Domain.Common;

namespace Store.Domain.Entities;

public class UserAddress : BaseEntity
{
    public int CityId { get; private set; }
    public string? RecipientFirstName { get; private set; }
    public string? RecipientLastName { get; private set; }
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public int UserId { get; private set; }
    public string Address { get; private set; } = default!;
    public string? PostalCode { get; private set; }
    public string PhoneNumber { get; private set; } = default!;


    public User User { get; private set; } = default!;
    public City City { get; private set; } = default!;
    public ICollection<OrderItem> OrderItems { get; private set; } = default!;


    public static UserAddress Create(string title, int userId, string address, string? postalCode, int cityId,
                                     string? recipientFirstName, string? recipientLastName, string phoneNumber)
        => new()
        {
            Title = title,
            UserId = userId,
            Address = address,
            PostalCode = postalCode,
            CityId = cityId,
            RecipientFirstName = recipientFirstName,
            RecipientLastName = recipientLastName,
            PhoneNumber = phoneNumber
        };

    public void Update(string title, bool isActive, string address, string? postalCode, int cityId,
                       string? recipientFirstName, string? recipientLastName, string phoneNumber)
    {
        Title = title;
        CityId = cityId;
        Address = address;
        IsActive = isActive;
        PostalCode = postalCode;
        PhoneNumber = phoneNumber;
        RecipientLastName = recipientLastName;
        RecipientFirstName = recipientFirstName;
    }
}