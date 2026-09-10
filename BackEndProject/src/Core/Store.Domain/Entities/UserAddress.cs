using Store.Domain.Common;

namespace Store.Domain.Entities;

public class UserAddress : BaseEntity
{
    public int? CityId { get; private set; }
    public string? RecipientFirstName { get; private set; }
    public string? RecipientLastName { get; private set; }
    public string Title { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public bool IsDefault { get; private set; }
    public int UserId { get; private set; }
    public string Address { get; private set; } = default!;
    public string? Apartment { get; private set; }
    public string? PostalCode { get; private set; }
    public string PhoneNumber { get; private set; } = default!;
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }


    public User User { get; private set; } = default!;
    public City? City { get; private set; } = default!;
    public ICollection<OrderItem> OrderItems { get; private set; } = default!;


    public static UserAddress Create(string title, int userId, string address, string? apartment, string? postalCode,
                                     int? cityId, string? recipientFirstName, string? recipientLastName,
                                     string phoneNumber, decimal? latitude, decimal? longitude, bool isDefault)
        => new()
        {
            Title = title,
            UserId = userId,
            Address = address,
            Apartment = apartment,
            PostalCode = postalCode,
            CityId = cityId,
            RecipientFirstName = recipientFirstName,
            RecipientLastName = recipientLastName,
            PhoneNumber = phoneNumber,
            Latitude = latitude,
            Longitude = longitude,
            IsDefault = isDefault
        };

    public void Update(string title, bool isActive, string address, string? apartment, string? postalCode, int? cityId,
                       string? recipientFirstName, string? recipientLastName, string phoneNumber,
                       decimal? latitude, decimal? longitude, bool isDefault)
    {
        Title = title;
        CityId = cityId;
        Address = address;
        Apartment = apartment;
        IsActive = isActive;
        IsDefault = isDefault;
        PostalCode = postalCode;
        PhoneNumber = phoneNumber;
        RecipientLastName = recipientLastName;
        RecipientFirstName = recipientFirstName;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void SetDefault(bool isDefault) => IsDefault = isDefault;
}
