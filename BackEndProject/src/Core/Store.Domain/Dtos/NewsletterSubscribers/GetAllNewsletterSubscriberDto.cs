using Store.Domain.Dtos.Pagination;
using Store.Domain.QueryFilters;

namespace Store.Domain.Dtos.NewsletterSubscribers;

public record GetAllNewsletterSubscriberRequestDto
{
    [QueryFilter]
    public int? LanguageId { get; init; }

    [QueryFilter(Operator = FilterOperator.Contains)]
    public string? Email { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllNewsletterSubscriberResponseDto
{
    public int Id { get; init; }
    public string Email { get; init; } = default!;
    public int LanguageId { get; init; }
    public DateTime SubscribedAtUtc { get; init; }
    public bool IsActive { get; init; }
}
