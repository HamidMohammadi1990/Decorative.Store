using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductOrderItemAttachmentTypes;

public record GetAllProductOrderItemAttachmentTypeRequestDto : IContentPolicyQueryDto<ProductOrderItemAttachmentType>
{
    [QueryFilter]
    public int? ProductId { get; init; }

    [QueryFilter]
    public int? OrderItemAttachmentTypeId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductOrderItemAttachmentType, bool>>? ContentFilter { get; set; }
}