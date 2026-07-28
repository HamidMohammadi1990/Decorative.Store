using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.CompanyStoryLikes.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Company Story Like For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("company-story-like")]
[ApiControllerCategory(ApiControllerCategory.Company)]
[ControllerInfo(PermissionType.ManageCompanyStoryLike, PermissionType.ManageCompanyGroup)]
public class CompanyStoryLikeController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCompanyStoryLike)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCompanyStoryLikeResponse>>> GetAll(GetAllCompanyStoryLikeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCompanyStoryLikeById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCompanyStoryLikeResponse?>> Get(GetCompanyStoryLikeRequest request)
        => await mediator.Send(request);
}
