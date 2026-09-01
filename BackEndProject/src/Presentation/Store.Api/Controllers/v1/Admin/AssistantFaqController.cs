using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.AssistantFaqs.Queries;
using Edition.Application.Features.AssistantFaqs.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management assistant FAQ items for admin
/// </summary>
[ApiVersion("1")]
[ControllerName("assistant-faq")]
[ApiControllerCategory(ApiControllerCategory.Cms)]
[ControllerInfo(PermissionType.ManageAssistantFaq, PermissionType.ManageAssistantFaqGroup)]
public class AssistantFaqController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListAssistantFaq)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllAssistantFaqResponse>>> GetAll(GetAllAssistantFaqRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetAssistantFaqById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetAssistantFaqResponse?>> Get(GetAssistantFaqRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateAssistantFaq)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateAssistantFaqResponse>> Create(CreateAssistantFaqRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateAssistantFaq)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateAssistantFaqRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteAssistantFaq)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteAssistantFaqRequest request)
        => await mediator.Send(request);
}
