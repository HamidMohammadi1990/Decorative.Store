using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.Sections.Queries;

public record SearchSectionResponse
{
    [JsonConverter(typeof(SectionEncryptor))]
    public int Id { get; init; }
    [JsonConverter(typeof(SectionTypeEncryptor))]
    public int SectionTypeId { get; init; }
    [JsonConverter(typeof(SectionNullableEncryptor))]
    public int? ParentId { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; set; }
    public string Url { get; init; } = default!;
    public string? ImageUrl { get; init; }
    public bool IsActive { get; init; }
}
