using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Companies.Queries;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Companies For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("company")]
[ApiControllerCategory(ApiControllerCategory.Company)]
[ControllerInfo(PermissionType.ManageCompany, PermissionType.ManageCompanyGroup)]
public class CompanyController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCompany)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCompanyResponse>>> GetAll(GetAllCompanyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCompanyById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCompanyResponse?>> Get(GetCompanyRequest request)
        => await mediator.Send(request);

}
