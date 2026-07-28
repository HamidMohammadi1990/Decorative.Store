using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Tags.Queries;

public record GetTagRequest : IRequest<OperationResult<GetTagResponse?>>
{
    [JsonConverter(typeof(TagEncryptor))]
    public int Id { get; set; }
}