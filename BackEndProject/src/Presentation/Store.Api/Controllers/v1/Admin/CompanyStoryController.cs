using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.CompanyStories.Queries;
using Edition.Application.Features.CompanyStories.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Company Story For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("company-story")]
[ApiControllerCategory(ApiControllerCategory.Company)]
[ControllerInfo(PermissionType.ManageCompanyStory, PermissionType.ManageCompanyGroup)]
public class CompanyStoryController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListCompanyStory)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllCompanyStoryResponse>>> GetAll(GetAllCompanyStoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetCompanyStoryById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetCompanyStoryResponse?>> Get(GetCompanyStoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateCompanyStory)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateCompanyStoryResponse>> Create(CreateCompanyStoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateCompanyStory)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateCompanyStoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ActivateCompanyStory)]
    [HttpPost("activate")]
    public async Task<ApiResult<OperationResult>> Activate(ActivateCompanyStoryRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeactivateCompanyStory)]
    [HttpPost("deactivate")]
    public async Task<ApiResult<OperationResult>> Deactivate(DeactivateCompanyStoryRequest request)
        => await mediator.Send(request);
}
