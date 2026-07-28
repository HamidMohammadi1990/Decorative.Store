using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPostTags.Queries;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Blog Post Tag
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-tag")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
public class BlogPostTagController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchBlogPostTagResponse>>> Search(SearchBlogPostTagRequest request)
        => await mediator.Send(request);
}