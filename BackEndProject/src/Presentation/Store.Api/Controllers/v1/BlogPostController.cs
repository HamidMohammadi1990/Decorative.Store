using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPosts.Queries;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Blog Post
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
public class BlogPostController
    (ISender mediator)
    : BaseApiController
{
    [HttpGet("detail")]
    public async Task<ApiResult<GetBlogPostDetailResponse>> Detail([FromQuery] string slug)
        => await mediator.Send(new GetBlogPostDetailRequest(slug));

    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchBlogPostResponse>>> Search(SearchBlogPostRequest request)
        => await mediator.Send(request);
}