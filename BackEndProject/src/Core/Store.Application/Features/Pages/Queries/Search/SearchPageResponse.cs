using Edition.Application.Common.Utilities.Security.Attributes;
using System.Text.Json.Serialization;
using Store.Domain.Enums;

namespace Edition.Application.Features.Pages.Queries;

public record SearchPageResponse
{
    [JsonConverter(typeof(PageEncryptor))]
    public int Id { get; init; }
    public string Slug { get; init; } = default!;
    public string Title { get; init; } = default!;
    public PageType Type { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
}
