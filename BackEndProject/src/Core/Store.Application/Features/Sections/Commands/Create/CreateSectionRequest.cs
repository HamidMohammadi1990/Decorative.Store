using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.Sections.Commands;

public record CreateSectionRequest : IRequest<OperationResult<CreateSectionResponse>>
{
    public int LanguageId { get; init; }
    [JsonConverter(typeof(SectionTypeEncryptor))]
    public int SectionTypeId { get; init; }
    [JsonConverter(typeof(SectionNullableEncryptor))]
    public int? ParentId { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public string Url { get; init; } = default!;
    public string? ImageUrl { get; init; }
    public DateTime? StartDateOnUtc { get; init; }
    public DateTime? EndDateOnUtc { get; init; }
    public bool IsActive { get; init; }
}
