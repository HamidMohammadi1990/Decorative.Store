using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPostComments.Queries;
using Edition.Application.Features.BlogPostComments.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Blog Post Comment For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-comment")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
[ControllerInfo(PermissionType.ManageBlogPostComment, PermissionType.ManageBlogPostGroup)]
public class BlogPostCommentController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListBlogPostComment)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllBlogPostCommentResponse>>> GetAll(GetAllBlogPostCommentRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetBlogPostCommentById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetBlogPostCommentResponse?>> Get(GetBlogPostCommentRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ApproveBlogPostComment)]
    [HttpPost("approve")]
    public async Task<ApiResult<OperationResult>> Approve(ApproveBlogPostCommentRequest request)
        => await mediator.Send(request);
}
