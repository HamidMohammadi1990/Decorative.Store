using Store.Common.Models;

namespace Edition.Application.Features.Categories.Commands;

public record CreateCategoryRequest : IRequest<OperationResult<CreateCategoryResponse>>
{
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Code { get; init; } = default!;
}
