namespace Store.Domain.Dtos.UserAddresses;

public record GetUserAddressDto
{
    public int Id { get; init; }
    public int? CityId { get; init; }
    public int? ProvinceId { get; init; }
    public string? CityName { get; init; }
    public string? RecipientFirstName { get; init; }
    public string? RecipientLastName { get; init; }
    public string Title { get; init; } = default!;
    public int UserId { get; init; }
    public string Address { get; init; } = default!;
    public string? Apartment { get; init; }
    public string? PostalCode { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public bool IsDefault { get; init; }
}
