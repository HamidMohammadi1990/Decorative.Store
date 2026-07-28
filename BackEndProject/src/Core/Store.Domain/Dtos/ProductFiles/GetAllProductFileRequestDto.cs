using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.ProductFiles;

public record GetAllProductFileRequestDto : IContentPolicyQueryDto<ProductFile>
{
    [QueryFilter(MemberPath = "productFile.Title", Operator = FilterOperator.Contains)]
    public string? Title { get; init; }

    [QueryFilter(MemberPath = "productFile.ProductId")]
    public int? ProductId { get; init; }

    [QueryFilter(MemberPath = "productFile.IsActive")]
    public bool? IsActive { get; init; }

    [QueryFilter(MemberPath = "productFile.IsMain")]
    public bool? IsMain { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<ProductFile, bool>>? ContentFilter { get; set; }
}
