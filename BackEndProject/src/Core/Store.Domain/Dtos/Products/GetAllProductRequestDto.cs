using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.Products;

public record GetAllProductRequestDto : IContentPolicyQueryDto<Product>
{
    public int? CategoryId { get; init; }
    public string? CategorySlug { get; set; }
    public int? SubCategoryId { get; init; }
    public string? SubCategorySlug { get; init; }
    public string? Slug { get; init; }
    public string? Title { get; init; }
    public string? ProductCode { get; init; }
    public bool? IsActive { get; set; }
    public PagedRequest Pagination { get; init; } = default!;
    public Expression<Func<Product, bool>>? ContentFilter { get; set; }
}
