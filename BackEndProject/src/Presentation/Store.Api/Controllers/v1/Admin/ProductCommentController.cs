using MediatR;
using Asp.Versioning;
using Store.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using Edition.Application.Features.ProductComments.Queries;
using Edition.Application.Features.ProductComments.Commands;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;

namespace Store.Api.Controllers.v1.Admin;

/// <summary>
/// Management Product Comments For Admin
/// </summary>
[ApiVersion("1")]
[ControllerName("product-comment")]
[ApiControllerCategory(ApiControllerCategory.Product)]
[ControllerInfo(PermissionType.ManageProductComment, PermissionType.ManageProductCommentGroup)]
public class ProductCommentController
    (ISender mediator)
    : BaseApiAdminController
{
    [ActionInfo(PermissionType.ListProductComment)]
    [HttpPost("get-all")]
    public async Task<ApiResult<PagedResult<GetAllProductCommentResponse>>> GetAll(GetAllProductCommentRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.GetProductCommentById)]
    [HttpPost("get")]
    public async Task<ApiResult<GetProductCommentResponse?>> Get(GetProductCommentRequest request)
        => await mediator.Send(request);

    [ActionInfo(PermissionType.ChangeProductCommentStatus)]
    [HttpPost("change-status")]
    public async Task<ApiResult<OperationResult>> ChangeStatus(ChangeStatusProductCommentRequest request)
        => await mediator.Send(request);
}
