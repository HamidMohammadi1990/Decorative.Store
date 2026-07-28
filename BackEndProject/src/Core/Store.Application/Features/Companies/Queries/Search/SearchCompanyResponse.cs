using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.Companies.Queries;

public record SearchCompanyResponse
{
    [JsonConverter(typeof(CompanyEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string? UserFirstName { get; init; } = default!;
    public string? UserLastName { get; init; } = default!;
    public string ProvinceName { get; init; } = default!;

    [JsonConverter(typeof(ProvinceEncryptor))]
    public int ProvinceId { get; init; }

    public string CityName { get; init; } = default!;

    [JsonConverter(typeof(CityEncryptor))]
    public int CityId { get; init; }

    public string Name { get; init; } = default!;
    public string Code { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string? Email { get; init; }
    public string PostalCode { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string? Description { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public float Latitude { get; init; }
    public float Longitude { get; init; }
}