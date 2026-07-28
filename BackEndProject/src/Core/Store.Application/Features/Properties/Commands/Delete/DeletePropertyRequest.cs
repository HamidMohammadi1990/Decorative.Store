using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Properties.Commands;

public record DeletePropertyRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PropertyEncryptor))]
    public int Id { get; init; }
}