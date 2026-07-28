namespace Store.Domain.Dtos.Companies;

public record GetAllCompanyResponseDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string? UserFirstName { get; init; } = default!;
    public string? UserLastName { get; init; } = default!;
    public string ProvinceName { get; init; } = default!;
    public int ProvinceId { get; init; }
    public string CityName { get; init; } = default!;
    public int CityId { get; init; }
    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string? Email { get; init; }
    public string PostalCode { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string? Description { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public bool IsActive { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
}