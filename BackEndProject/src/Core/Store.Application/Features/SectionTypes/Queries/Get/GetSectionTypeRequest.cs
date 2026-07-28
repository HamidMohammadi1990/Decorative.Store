using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.SectionTypes.Queries;

public record GetSectionTypeRequest : IRequest<OperationResult<GetSectionTypeResponse?>>
{
    [JsonConverter(typeof(SectionTypeEncryptor))]
    public int Id { get; init; }
}
