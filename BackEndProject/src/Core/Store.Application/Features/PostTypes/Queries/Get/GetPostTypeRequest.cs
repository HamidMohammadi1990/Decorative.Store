using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PostTypes.Queries;

public record GetPostTypeRequest : IRequest<OperationResult<GetPostTypeResponse?>>
{
    [JsonConverter(typeof(PostTypeEncryptor))]
    public int Id { get; init; }
}