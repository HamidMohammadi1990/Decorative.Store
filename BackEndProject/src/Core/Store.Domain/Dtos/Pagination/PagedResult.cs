namespace Store.Domain.Dtos.Pagination;

public record PagedResult
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}

public record PagedResult<T> : PagedResult
{
    public List<T> Items { get; init; } = default!;


    public static PagedResult<T> Create(List<T> items, PagedRequest pagedRequest, int totalCount)
        => new()
        {
            Items = items,
            PageNumber = pagedRequest.PageNumber,
            PageSize = pagedRequest.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pagedRequest.PageSize)
        };

    public static PagedResult<T> Create(List<T> items, PagedResult pagedResult)
        => new()
        {
            Items = items,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = (int)Math.Ceiling(pagedResult.TotalCount / (double)pagedResult.PageSize)
        };
}