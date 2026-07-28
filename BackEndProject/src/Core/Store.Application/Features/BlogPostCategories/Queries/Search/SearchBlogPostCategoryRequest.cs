using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public record SearchBlogPostCategoryRequest : ContentPolicyRequest<BlogPostCategory>, IRequest<OperationResult<PagedResult<SearchBlogPostCategoryResponse>>>
{
    public string? Title { get; set; }
    public string? Slug { get; set; }

    public PagedRequest Pagination { get; init; } = default!;
}