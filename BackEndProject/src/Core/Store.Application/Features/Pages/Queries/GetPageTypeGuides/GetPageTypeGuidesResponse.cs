using Store.Domain.Enums;

namespace Edition.Application.Features.Pages.Queries;

public record GetPageTypeGuidesResponse
{
    public PageType Type { get; init; }
    public string AdminDescription { get; init; } = default!;
}
