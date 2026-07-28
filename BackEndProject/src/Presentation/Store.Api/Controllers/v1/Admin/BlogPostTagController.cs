using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPostTags.Queries;
using Edition.Application.Features.BlogPostTags.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Blog Post Tag For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-tag")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
[ControllerInfo(PermissionType.ManageBlogPostTag, PermissionType.ManageBlogPostGroup)]
public class BlogPostTagController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListBlogPostTag)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllBlogPostTagResponse>>> GetAll(GetAllBlogPostTagRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetBlogPostTagById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetBlogPostTagResponse?>> Get(GetBlogPostTagRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateBlogPostTag)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateBlogPostTagResponse>> Create(CreateBlogPostTagRequest request)
       => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateBlogPostTag)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateBlogPostTagRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteBlogPostTag)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteBlogPostTagRequest request)
        => await mediator.Send(request);
}
