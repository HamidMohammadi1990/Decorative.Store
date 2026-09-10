using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.UserAddresses.Queries;

public record GetUserAddressesResponse
{
    [JsonConverter(typeof(UserAddressEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CityNullableEncryptor))]
    public int? CityId { get; init; }

    [JsonConverter(typeof(ProvinceNullableEncryptor))]
    public int? ProvinceId { get; init; }

    public string? CityName { get; init; }

    public string? RecipientFirstName { get; init; }
    public string? RecipientLastName { get; init; }
    public string Title { get; init; } = default!;

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }
    public string Address { get; init; } = default!;
    public string? Apartment { get; init; }
    public string? PostalCode { get; init; }
    public string PhoneNumber { get; init; } = default!;
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public bool IsDefault { get; init; }
}
