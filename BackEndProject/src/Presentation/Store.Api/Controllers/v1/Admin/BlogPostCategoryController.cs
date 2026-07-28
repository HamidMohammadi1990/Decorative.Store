using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.BlogPostCategories.Commands;
using Edition.Application.Features.BlogPostCategories.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Blog Post Category For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("blog-post-category")]
[ApiControllerCategory(ApiControllerCategory.Blog)]
[ControllerInfo(PermissionType.ManageBlogPostCategory, PermissionType.ManageBlogPostGroup)]
public class BlogPostCategoryController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListBlogPostCategory)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllBlogPostCategoryResponse>>> GetAll(GetAllBlogPostCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetBlogPostCategoryById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetBlogPostCategoryResponse?>> Get(GetBlogPostCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateBlogPostCategory)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateBlogPostCategoryResponse>> Create(CreateBlogPostCategoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateBlogPostCategory)]
    [HttpPost("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateBlogPostCategoryRequest request)
        => await mediator.Send(request);
}
