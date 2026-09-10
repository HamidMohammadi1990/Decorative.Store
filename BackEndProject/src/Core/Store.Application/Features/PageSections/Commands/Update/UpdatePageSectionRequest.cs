using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;

namespace Edition.Application.Features.PageSections.Commands;

public record UpdatePageSectionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PageSectionEncryptor))]
    public int Id { get; init; }
    [JsonConverter(typeof(PageEncryptor))]
    public int PageId { get; init; }
    [JsonConverter(typeof(SectionEncryptor))]
    public int SectionId { get; init; }
    public int Priority { get; init; }
    public string? AdminDescription { get; init; }
}
