using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPostCategories.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Blog Post Category
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-category")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
public class BlogPostCategoryController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchBlogPostCategoryResponse>>> Search(SearchBlogPostCategoryRequest request)
        => await mediator.Send(request);
}