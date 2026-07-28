using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public record GetAllProductOrderItemAttachmentTypeRequest : ContentPolicyRequest<ProductOrderItemAttachmentType>, IRequest<OperationResult<PagedResult<GetAllProductOrderItemAttachmentTypeResponse>>>
{
    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }
    public int? OrderItemAttachmentTypeId { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
