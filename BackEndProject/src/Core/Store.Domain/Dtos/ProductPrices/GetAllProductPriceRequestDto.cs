using System.Linq.Expressions;
using Store.Domain.QueryFilters;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductPrices;

public record GetAllProductPriceRequestDto : IContentPolicyQueryDto<ProductPrice>
{
    [QueryFilter(MemberPath = "productPrice.CompanyId")]
    public int? CompanyId { get; init; }

    [QueryFilter(MemberPath = "user.Id")]
    public int? UserId { get; init; }

    [QueryFilter(MemberPath = "productPrice.ProductId")]
    public int? ProductId { get; init; }

    [QueryFilter(MemberPath = "productPrice.IsActive")]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductPrice, bool>>? ContentFilter { get; set; }
}
