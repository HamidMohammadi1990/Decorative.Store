using Store.Common.Models;

namespace Edition.Application.Features.BlogPostCategories.Commands;

public record CreateBlogPostCategoryRequest : IRequest<OperationResult<CreateBlogPostCategoryResponse>>
{
    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
}