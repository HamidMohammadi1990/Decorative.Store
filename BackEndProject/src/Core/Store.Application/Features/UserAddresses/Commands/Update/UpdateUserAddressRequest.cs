using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserAddresses.Commands;

public class UpdateUserAddressRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(UserAddressEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public bool IsActive { get; init; }
    public string Address { get; init; } = default!;
    public string? PostalCode { get; init; }

    [JsonConverter(typeof(CityNullableEncryptor))]
    public int? CityId { get; init; }
    public string RecipientFirstName { get; init; } = default!;
    public string RecipientLastName { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
}