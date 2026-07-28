using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPosts.Queries;
using Edition.Application.Features.BlogPosts.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Blog Post For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
[ControllerInfo(PermissionType.ManageBlogPost, PermissionType.ManageBlogPostGroup)]
public class BlogPostController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListBlogPost)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllBlogPostResponse>>> GetAll(GetAllBlogPostRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetBlogPostById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetBlogPostResponse?>> Get(GetBlogPostRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateBlogPost)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateBlogPostResponse>> Create(CreateBlogPostRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateBlogPost)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateBlogPostRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.PublishBlogPost)]
    [HttpPost("publish")]
    public async Task<ApiResult<OperationResult>> Publish(PublishBlogPostRequest request)
        => await mediator.Send(request);
}
