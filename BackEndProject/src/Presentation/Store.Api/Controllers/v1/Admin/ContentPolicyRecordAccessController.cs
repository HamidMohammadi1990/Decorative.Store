using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ContentPolicyRecordAccesses.Queries;
using Edition.Application.Features.ContentPolicyRecordAccesses.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

[ApiVersion("1")]
[ControllerName("content-policy-record-access")]
[ApiControllerCategory(ApiControllerCategory.ContentPolicy)]
[ControllerInfo(PermissionType.ManageContentPolicyRecordAccessGroup, PermissionType.ManageContentPolicyRecordAccess)]
public class ContentPolicyRecordAccessController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListContentPolicyRecordAccess)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllContentPolicyRecordAccessResponse>>> GetAll(GetAllContentPolicyRecordAccessRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetContentPolicyRecordAccessById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetContentPolicyRecordAccessResponse?>> Get(GetContentPolicyRecordAccessRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateContentPolicyRecordAccess)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateContentPolicyRecordAccessResponse>> Create(CreateContentPolicyRecordAccessRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.SetContentPolicyRecordAccess)]
    [HttpPut("set")]
    public async Task<ApiResult<OperationResult>> Set(SetContentPolicyRecordAccessRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteContentPolicyRecordAccess)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteContentPolicyRecordAccessRequest request)
        => await mediator.Send(request);
}
