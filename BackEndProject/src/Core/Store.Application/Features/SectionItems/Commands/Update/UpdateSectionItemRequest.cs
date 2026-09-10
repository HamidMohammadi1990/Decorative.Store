using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.SectionItems.Commands;

public record UpdateSectionItemRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(SectionItemEncryptor))]
    public int Id { get; init; }
    public int LanguageId { get; init; }
    [JsonConverter(typeof(SectionEncryptor))]
    public int SectionId { get; init; }
    public string Title { get; init; } = default!;
    public int Priority { get; init; }
    public string? Icon { get; init; }
    public string? ImageUrl { get; init; }
    public string? Url { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public string? AdminDescription { get; init; }
}
