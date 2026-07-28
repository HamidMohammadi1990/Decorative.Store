using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ContentPolicyRules.Queries;
using Edition.Application.Features.ContentPolicyRules.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

[ApiVersion("1")]
[ControllerName("content-policy-rule")]
[ApiControllerCategory(ApiControllerCategory.ContentPolicy)]
[ControllerInfo(PermissionType.ManageContentPolicyRule, PermissionType.ManageContentPolicyRuleGroup)]
public class ContentPolicyRuleController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListContentPolicyRule)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllContentPolicyRuleResponse>>> GetAll(GetAllContentPolicyRuleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetContentPolicyRuleById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetContentPolicyRuleResponse?>> Get(GetContentPolicyRuleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateContentPolicyRule)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateContentPolicyRuleResponse>> Create(CreateContentPolicyRuleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateContentPolicyRule)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateContentPolicyRuleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteContentPolicyRule)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteContentPolicyRuleRequest request)
        => await mediator.Send(request);
}
