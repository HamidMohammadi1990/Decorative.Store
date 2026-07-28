using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductDescriptions.Queries;
using Edition.Application.Features.ProductDescriptions.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Descriptions For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-description")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductDescription, PermissionType.ManageProductGroup)]
public class ProductDescriptionController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductDescription)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductDescriptionResponse>>> GetAll(GetAllProductDescriptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductDescriptionById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductDescriptionResponse?>> Get(GetProductDescriptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductDescription)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductDescriptionResponse>> Create(CreateProductDescriptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductDescription)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductDescriptionRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProductDescription)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductDescriptionRequest request)
        => await mediator.Send(request);
}