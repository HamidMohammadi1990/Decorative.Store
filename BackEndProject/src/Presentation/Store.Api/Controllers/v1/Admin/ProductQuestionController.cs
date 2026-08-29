using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductQuestions.Queries;
using Edition.Application.Features.ProductQuestions.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Questions For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-question")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductQuestion, PermissionType.ManageProductQuestionGroup)]
public class ProductQuestionController(ISender mediator) : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductQuestion)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductQuestionResponse>>> GetAll(GetAllProductQuestionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ChangeProductQuestionStatus)]
    [HttpPost("change-status")]
    public async Task<ApiResult<OperationResult>> ChangeStatus(ChangeStatusProductQuestionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.AnswerProductQuestion)]
    [HttpPost("answer")]
    public async Task<ApiResult<OperationResult>> Answer(AnswerProductQuestionRequest request)
        => await mediator.Send(request);
}
