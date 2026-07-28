using System.Linq.Expressions;
using Edition.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductPropertyPrices;

public record GetAllProductPropertyPriceRequestDto : IContentPolicyQueryDto<ProductPropertyPrice>
{
    [QueryFilter(MemberPath = "productPropertyPrice.CompanyId")]
    public int? CompanyId { get; init; }

    [QueryFilter(MemberPath = "company.UserId")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "productPropertyPrice.ProductPropertyId")]
    public int? ProductPropertyId { get; init; }

    [QueryFilter(MemberPath = "product.Id")]
    public int? ProductId { get; init; }

    [QueryFilter(MemberPath = "productPropertyPrice.IsActive")]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductPropertyPrice, bool>>? ContentFilter { get; set; }
}
