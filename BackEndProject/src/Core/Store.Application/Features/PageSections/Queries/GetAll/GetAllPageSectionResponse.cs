using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;

namespace Edition.Application.Features.PageSections.Queries;

public record GetAllPageSectionResponse
{
    [JsonConverter(typeof(PageSectionEncryptor))]
    public int Id { get; init; }
    [JsonConverter(typeof(PageEncryptor))]
    public int PageId { get; init; }
    [JsonConverter(typeof(SectionEncryptor))]
    public int SectionId { get; init; }
    public int Priority { get; init; }
    public string PageTitle { get; init; } = string.Empty;
    public string PageSlug { get; init; } = string.Empty;
    public string SectionTitle { get; init; } = string.Empty;
    public string SectionTypeName { get; init; } = string.Empty;
}
