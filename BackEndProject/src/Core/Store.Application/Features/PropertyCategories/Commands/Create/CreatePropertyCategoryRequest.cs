using Store.Common.Models;

namespace Edition.Application.Features.PropertyCategories.Commands;

public record CreatePropertyCategoryRequest : IRequest<OperationResult<CreatePropertyCategoryResponse>>
{
    public int LanguageId { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
}
