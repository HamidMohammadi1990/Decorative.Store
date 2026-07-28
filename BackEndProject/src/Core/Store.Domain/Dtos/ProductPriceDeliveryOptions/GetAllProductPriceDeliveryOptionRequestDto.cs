using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductPriceDeliveryOptions;

public record GetAllProductPriceDeliveryOptionRequestDto : IContentPolicyQueryDto<ProductPriceDeliveryOption>
{
    [QueryFilter(MemberPath = "Company.Id")]
    public int? CompanyId { get; init; }

    [QueryFilter(MemberPath = "ProductPriceDeliveryOption.ProductPriceId")]
    public int? ProductPriceId { get; init; }

    [QueryFilter(MemberPath = "ProductPriceDeliveryOption.DeliveryOptionId")]
    public int? DeliveryOptionId { get; init; }

    [QueryFilter(MemberPath = "ProductPriceDeliveryOption.IsActive")]
    public bool? IsActive { get; init; }

    public int? UserId { get; init; }
    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductPriceDeliveryOption, bool>>? ContentFilter { get; set; }
}
