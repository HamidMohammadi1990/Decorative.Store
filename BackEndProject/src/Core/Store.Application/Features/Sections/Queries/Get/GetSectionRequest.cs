using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.Sections.Queries;

public record GetSectionRequest : IRequest<OperationResult<GetSectionResponse?>>
{
    [JsonConverter(typeof(SectionEncryptor))]
    public int Id { get; init; }
}
