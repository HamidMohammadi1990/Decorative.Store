using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Properties.Queries;

public record GetPropertyRequest : IRequest<OperationResult<GetPropertyResponse?>>
{
    [JsonConverter(typeof(PropertyEncryptor))]
    public int Id { get; init; }
}