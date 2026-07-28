using Asp.Versioning;
using Store.Api.Attributes;
using Edition.Application.Features.ProductPropertyRules.Queries;
using Edition.Application.Features.ProductPropertyRules.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Property Rules For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-property-rule")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductPropertyRule, PermissionType.ManageProductGroup)]
public class ProductPropertyRuleController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductPropertyRule)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductPropertyRuleResponse>>> GetAll(GetAllProductPropertyRuleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductPropertyRuleById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductPropertyRuleResponse?>> Get(GetProductPropertyRuleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductPropertyRule)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductPropertyRuleResponse>> Create(CreateProductPropertyRuleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductPropertyRule)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductPropertyRuleRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProductPropertyRule)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductPropertyRuleRequest request)
        => await mediator.Send(request);
}
