using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.PageSections.Queries;

public record GetPageSectionRequest : IRequest<OperationResult<GetPageSectionResponse?>>
{
    [JsonConverter(typeof(PageSectionEncryptor))]
    public int Id { get; init; }
}
