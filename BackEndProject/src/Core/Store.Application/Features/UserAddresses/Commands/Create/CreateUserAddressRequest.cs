using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserAddresses.Commands;

public record CreateUserAddressRequest : IRequest<OperationResult<CreateUserAddressResponse>>
{
    public string Title { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string? Apartment { get; init; }
    public string? PostalCode { get; init; }

    [JsonConverter(typeof(CityNullableEncryptor))]
    public int? CityId { get; init; }
    public string RecipientFirstName { get; init; } = default!;
    public string RecipientLastName { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public decimal? Latitude { get; init; }
    public decimal? Longitude { get; init; }
    public bool IsDefault { get; init; }
}
