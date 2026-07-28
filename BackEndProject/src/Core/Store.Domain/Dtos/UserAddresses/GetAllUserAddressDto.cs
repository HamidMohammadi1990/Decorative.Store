namespace Store.Domain.Dtos.UserAddresses;

public record GetAllUserAddressDto
{
    public int Id { get; init; }
    public int CityId { get; init; }
    public string CityName { get; init; } = default!;
    public string? RecipientFirstName { get; init; }
    public string? RecipientLastName { get; init; }
    public string Title { get; init; } = default!;
    public int UserId { get; init; }
    public string UserName { get; init; } = default!;
    public string? UserFirstName { get; init; }
    public string? UserLastName { get; init; }
    public string Address { get; init; } = default!;
    public string? PostalCode { get; init; }
    public string PhoneNumber { get; init; } = default!;
}