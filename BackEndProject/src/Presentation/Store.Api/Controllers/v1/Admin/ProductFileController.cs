using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductFiles.Queries;
using Edition.Application.Features.ProductFiles.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Files For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-file")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductFile, PermissionType.ManageProductGroup)]
public class ProductFileController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductFile)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductFileResponse>>> GetAll(GetAllProductFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductFileById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductFileResponse?>> Get(GetProductFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.CreateProductFileRange)]
    [HttpPost("create-range")]
    [RequestSizeLimit(5_242_880)]
    public async Task<ApiResult<List<CreateProductFileResponse>>> CreateRange([FromForm] CreateProductFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.DeleteProductFile)]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductFileStatus)]
    [HttpPut("status")]
    public async Task<ApiResult<OperationResult>> UpdateStatus(UpdateStatusProductFileRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.UpdateProductFileStatus)]
    [HttpPut("main")]
    public async Task<ApiResult<OperationResult>> SetMain(UpdateSetMainProductFileRequest request)
        => await mediator.Send(request);
}
