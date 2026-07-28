using Store.Common.Models;

namespace Edition.Application.Features.SubCategories.Commands;

public record CreateSubCategoryRequest : IRequest<OperationResult<CreateSubCategoryResponse>>
{
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Code { get; set; } = default!;
    public int CategoryId { get; set; }
}