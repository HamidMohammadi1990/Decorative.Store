using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPostLikes.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Blog Post Like For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-like")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
[ControllerInfo(PermissionType.ManageBlogPostLike, PermissionType.ManageBlogPostGroup)]
public class BlogPostLikeController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListBlogPostLike)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllBlogPostLikeResponse>>> GetAll(GetAllBlogPostLikeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetBlogPostLikeById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetBlogPostLikeResponse?>> Get(GetBlogPostLikeRequest request)
        => await mediator.Send(request);
}
