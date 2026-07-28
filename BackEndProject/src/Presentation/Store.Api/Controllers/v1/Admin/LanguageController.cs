using Asp.Versioning;
using Edition.Api.Attributes;
using Edition.Application.Features.Languages.Commands;
using Edition.Application.Features.Languages.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.WebFramework.Api;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management languages for admin.
/// </summary>
[ApiVersion("1")]
[ControllerName("language")]
[ApiControllerCategory(ApiControllerCategory.Localization)]
[ControllerInfo(PermissionType.ManageLanguage, PermissionType.ManageLanguageGroup)]
public class LanguageController(ISender mediator) : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListLanguage)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllLanguageResponse>>> GetAll(GetAllLanguageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetLanguageById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetLanguageResponse?>> Get(GetLanguageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateLanguage)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateLanguageResponse>> Create(CreateLanguageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateLanguage)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateLanguageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.SetDefaultLanguage)]
    [HttpPut("set-default")]
    public async Task<ApiResult<OperationResult>> SetDefault(SetDefaultLanguageRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteLanguage)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteLanguageRequest request)
        => await mediator.Send(request);
}
