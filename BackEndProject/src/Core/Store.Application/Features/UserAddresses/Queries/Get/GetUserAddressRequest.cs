using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserAddresses.Queries;

public class GetUserAddressRequest : IRequest<OperationResult<GetUserAddressResponse?>>
{
    [JsonConverter(typeof(UserAddressEncryptor))]
    public int Id { get; init; }
}