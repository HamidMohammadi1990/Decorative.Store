using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Tags.Commands;

public record DeleteTagRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(TagEncryptor))]
    public int Id { get; init; }
}