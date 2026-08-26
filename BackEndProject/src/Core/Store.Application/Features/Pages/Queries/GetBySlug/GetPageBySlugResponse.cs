using Store.Domain.Enums;

namespace Edition.Application.Features.Pages.Queries;

public record GetPageBySlugResponse
{
    public bool NotFound { get; init; }
    public string Slug { get; init; } = default!;
    public string Title { get; init; } = default!;
    public PageType Type { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
    public IReadOnlyList<PageSectionRenderResponse> Sections { get; init; } = [];
}

public record PageSectionRenderResponse
{
    public int SectionTypeId { get; init; }
    public string SectionTypeName { get; init; } = default!;
    public int Priority { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public string Url { get; init; } = default!;
    public string? ImageUrl { get; init; }
    public IReadOnlyList<SectionItemRenderResponse> Items { get; init; } = [];
}

public record SectionItemRenderResponse
{
    public string Title { get; init; } = default!;
    public int Priority { get; init; }
    public string? Icon { get; init; }
    public string? ImageUrl { get; init; }
    public string? Url { get; init; }
    public string? Description { get; init; }
}
