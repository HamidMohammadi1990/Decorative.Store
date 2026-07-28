using Asp.Versioning;
using Store.Api.Attributes;
using Edition.Application.Features.ProductProperties.Queries;
using Edition.Application.Features.ProductProperties.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Properties For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-property")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductProperty, PermissionType.ManageProductGroup)]
public class ProductPropertyController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductProperty)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductPropertyResponse>>> GetAll(GetAllProductPropertyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductPropertyById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductPropertyResponse?>> Get(GetProductPropertyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductProperty)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductPropertyResponse>> Create(CreateProductPropertyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductProperty)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductPropertyRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProductProperty)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductPropertyRequest request)
        => await mediator.Send(request);
}
