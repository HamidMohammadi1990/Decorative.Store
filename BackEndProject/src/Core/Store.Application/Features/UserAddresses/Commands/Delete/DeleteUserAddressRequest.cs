using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.UserAddresses.Commands;

public class DeleteUserAddressRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(UserAddressEncryptor))]
    public int Id { get; init; }
}