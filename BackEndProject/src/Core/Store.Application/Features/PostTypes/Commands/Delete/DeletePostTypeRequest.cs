using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PostTypes.Commands;

public record DeletePostTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PostTypeEncryptor))]
    public int Id { get; init; }
}