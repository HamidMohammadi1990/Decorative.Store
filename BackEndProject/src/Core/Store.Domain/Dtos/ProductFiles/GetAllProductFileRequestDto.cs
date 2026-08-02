using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Dtos.ProductFiles;

public record GetAllProductFileRequestDto : IContentPolicyQueryDto<ProductFile>
{
    public string? Title { get; init; }
    public int? ProductId { get; init; }
    public bool? IsActive { get; init; }
    public bool? IsMain { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
    public Expression<Func<ProductFile, bool>>? ContentFilter { get; set; }
}
