using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Pages.Commands;

public record CreatePageRequest : IRequest<OperationResult<CreatePageResponse>>
{
    public string Slug { get; init; } = default!;
    public string Title { get; init; } = default!;
    public PageType Type { get; init; }
    public bool IsActive { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
}
