using Store.Common.Models;

namespace Edition.Application.Features.PropertyCategories.Commands;

public record CreatePropertyCategoryRequest : IRequest<OperationResult<CreatePropertyCategoryResponse>>
{
    public string Title { get; init; } = default!;
}