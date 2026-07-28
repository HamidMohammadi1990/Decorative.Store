using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.Products;

public record GetAllProductRequestDto : IContentPolicyQueryDto<Product>
{
    [QueryFilter(MemberPath = "companyProduct.CompanyId")]
    public int? CompanyId { get; set; }

    [QueryFilter(MemberPath = "category.Id")]
    public int? CategoryId { get; init; }

    [QueryFilter(MemberPath = "category.Slug")]
    public string? CategorySlug { get; set; }

    [QueryFilter(MemberPath = "product.SubCategoryId")]
    public int? SubCategoryId { get; init; }

    [QueryFilter(MemberPath = "subCategory.Slug")]
    public string? SubCategorySlug { get; init; }

    [QueryFilter(MemberPath = "product.Slug")]
    public string? Slug { get; init; }

    [QueryFilter(MemberPath = "product.Title", Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(MemberPath = "product.ProductCode", Operator = FilterOperator.Contains)]
    public string? ProductCode { get; init; }

    [QueryFilter(MemberPath = "product.IsActive")]
    public bool? IsActive { get; set; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<Product, bool>>? ContentFilter { get; set; }
}
