using Asp.Versioning;
using Edition.Api.Attributes;
using Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;
using Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Order Item Attachment Types For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-order-item-attachment-type")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductOrderItemAttachmentType, PermissionType.ManageProductGroup)]
public class ProductOrderItemAttachmentTypeController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductOrderItemAttachmentType)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductOrderItemAttachmentTypeResponse>>> GetAll(GetAllProductOrderItemAttachmentTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductOrderItemAttachmentTypeById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductOrderItemAttachmentTypeResponse?>> Get(GetProductOrderItemAttachmentTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductOrderItemAttachmentType)]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductOrderItemAttachmentTypeResponse>> Create(CreateProductOrderItemAttachmentTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductOrderItemAttachmentType)]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductOrderItemAttachmentTypeRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProductOrderItemAttachmentType)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductOrderItemAttachmentTypeRequest request)
        => await mediator.Send(request);
}
