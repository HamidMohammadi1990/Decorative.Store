using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Edition.Application.Features.ProductComments.Queries;
using Edition.Application.Features.ProductComments.Commands;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Product Comments
/// </summary>
[ApiVersion("1")]
[ControllerName("product-comment")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductCommentController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProductCommentResponse>>> Search(SearchProductCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPost("create")]
    public async Task<ApiResult<CreateProductCommentResponse>> Create(CreateProductCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpPut("update")]
    public async Task<ApiResult<OperationResult>> Update(UpdateProductCommentRequest request)
        => await mediator.Send(request);

    [Authorize]
    [HttpDelete("delete")]
    public async Task<ApiResult<OperationResult>> Delete(DeleteProductCommentRequest request)
        => await mediator.Send(request);
}