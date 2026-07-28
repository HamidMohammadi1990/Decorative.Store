using Asp.Versioning;
using Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using Store.Api.Attributes;
using Store.WebFramework.Api;
using Store.Common.Enums;
using Store.Domain.Dtos.Pagination;

namespace Store.Api.Controllers.v1;

/// <summary>
/// Management Product Order Item Attachment Types
/// </summary>
[ApiVersion("1")]
[ControllerName("product-order-item-attachment-type")]
[ApiControllerCategory(ApiControllerCategory.Product)]
public class ProductOrderItemAttachmentTypeController
    (ISender mediator)
    : BaseApiController
{
    [HttpPost("search")]
    public async Task<ApiResult<PagedResult<SearchProductOrderItemAttachmentTypeResponse>>> Search(SearchProductOrderItemAttachmentTypeRequest request)
        => await mediator.Send(request);
}
