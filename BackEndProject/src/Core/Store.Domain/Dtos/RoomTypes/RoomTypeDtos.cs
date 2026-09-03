using System.Linq.Expressions;
using Store.Domain.Dtos.ContentPolicies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.RoomTypes;

public record GetAllRoomTypeRequestDto : IContentPolicyQueryDto<RoomType>
{
    public int? LanguageId { get; init; }

    public string? Title { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Code { get; init; }

    [QueryFilter]
    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;

    public Expression<Func<RoomType, bool>>? ContentFilter { get; set; }
}

public record GetAllRoomTypeResponseDto
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public int Priority { get; init; }
    public bool IsActive { get; init; }
    public string? Title { get; init; }
    public int? LanguageId { get; init; }
}

public record ListRoomTypeResponseDto
{
    public int Id { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string ImageFileName { get; init; } = default!;
    public int Priority { get; init; }
}
