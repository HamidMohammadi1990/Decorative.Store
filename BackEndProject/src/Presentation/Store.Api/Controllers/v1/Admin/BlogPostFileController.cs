using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPostFiles.Queries;
using Edition.Application.Features.BlogPostFiles.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Blog Post Files For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-file")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
[ControllerInfo(PermissionType.ManageBlogPostFile, PermissionType.ManageBlogPostGroup)]
public class BlogPostFileController(ISender mediator) : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListBlogPostFile)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllBlogPostFileResponse>>> GetAll(GetAllBlogPostFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetBlogPostFileById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetBlogPostFileResponse?>> Get(GetBlogPostFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateBlogPostFileRange)]
    [HttpPost("create-range")]
    [RequestSizeLimit(5_242_880)]
    public async Task<ApiResult<List<CreateBlogPostFileResponse>>> CreateRange([FromForm] CreateBlogPostFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteBlogPostFile)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteBlogPostFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateBlogPostFileStatus)]
    [HttpPut("status")]
    public async Task<ApiResult<OperationResult>> UpdateStatus(UpdateStatusBlogPostFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateBlogPostFileStatus)]
    [HttpPut("main")]
    public async Task<ApiResult<OperationResult>> SetMain(UpdateSetMainBlogPostFileRequest request)
        => await mediator.Send(request);
}
