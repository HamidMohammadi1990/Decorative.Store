using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.SectionItems.Queries;

public record GetSectionItemRequest : IRequest<OperationResult<GetSectionItemResponse?>>
{
    [JsonConverter(typeof(SectionItemEncryptor))]
    public int Id { get; init; }
}
