namespace Store.Domain.Dtos.Pagination;

public record PagedRequest
{
    private const int MaxPageSize = 100;
    
    public int PageSize
    {
        get => field;
        set => field = value > MaxPageSize ? MaxPageSize : value;
    }
    public int PageNumber { get; init; } = 1;
}