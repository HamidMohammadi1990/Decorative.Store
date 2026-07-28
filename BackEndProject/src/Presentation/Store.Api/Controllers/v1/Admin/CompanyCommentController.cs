using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.CompanyComments.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Company Comment For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("company-comment")]
[ApiControllerCategory(ApiControllerCategory.Company)]
[ControllerInfo(PermissionType.ManageCompanyComment, PermissionType.ManageCompanyGroup)]
public class CompanyCommentController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCompanyComment)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCompanyCommentResponse>>> GetAll(GetAllCompanyCommentRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCompanyCommentById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCompanyCommentResponse>> Get(GetCompanyCommentRequest request)
        => await mediator.Send(request);
}
