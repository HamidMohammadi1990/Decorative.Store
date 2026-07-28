using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Edition.Domain.Dtos.Pagination;
using Edition.Application.Features.BlogPostLikes.Queries;
using Edition.Application.Features.BlogPostLikes.Commands;

using Edition.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Blog Post Like
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-like")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
public class BlogPostLikeController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("create")]
    public async Task<ApiResult<CreateBlogPostLikeResponse>> Create(CreateBlogPostLikeRequest request)
        => await mediator.Send(request);
}