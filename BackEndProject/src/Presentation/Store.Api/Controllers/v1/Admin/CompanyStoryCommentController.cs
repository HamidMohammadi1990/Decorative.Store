using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.CompanyStoryComments.Queries;
using Edition.Application.Features.CompanyStoryComments.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Company Story Comment For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("company-story-comment")]
[ApiControllerCategory(ApiControllerCategory.Company)]
[ControllerInfo(PermissionType.ManageCompanyStoryComment, PermissionType.ManageCompanyGroup)]
public class CompanyStoryCommentController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCompanyStoryComment)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCompanyStoryCommentResponse>>> GetAll(GetAllCompanyStoryCommentRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCompanyStoryCommentById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCompanyStoryCommentResponse?>> Get(GetCompanyStoryCommentRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ApproveCompanyStoryComment)]
    [HttpPost("approve")]
    public async Task<ApiResult<OperationResult>> Approve(ApproveCompanyStoryCommentRequest request)
        => await mediator.Send(request);
}
