using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.SectionItems.Commands;

public record CreateSectionItemRequest : IRequest<OperationResult<CreateSectionItemResponse>>
{
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
}
