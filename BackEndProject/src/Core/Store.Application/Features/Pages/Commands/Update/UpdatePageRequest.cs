using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Pages.Commands;

public record UpdatePageRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PageEncryptor))]
    public int Id { get; init; }

    public int LanguageId { get; init; }
    public string Slug { get; init; } = default!;
    public string Title { get; init; } = default!;
    public PageType Type { get; init; }
    public bool IsActive { get; init; }
    public string? AdminDescription { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
}
