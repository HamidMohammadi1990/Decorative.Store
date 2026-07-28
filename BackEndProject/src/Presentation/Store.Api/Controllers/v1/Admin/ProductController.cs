using MediatR;
using Asp.Versioning;
using Edition.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.Products.Queries;
using Edition.Application.Features.Products.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Products For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProduct, PermissionType.ManageProductGroup)]
public class ProductController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.GetProductById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductResponse?>> Get(GetProductRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ListProduct)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductResponse>>> GetAll(GetAllProductRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProduct)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductResponse>> Create(CreateProductRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProduct)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProduct)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductRequest request)
        => await mediator.Send(request);
}
