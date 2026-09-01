using Store.Common.Models;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.NewsletterSubscribers.Queries;

public record GetAllNewsletterSubscriberRequest
    : IRequest<OperationResult<PagedResult<GetAllNewsletterSubscriberResponse>>>
{
    public int? LanguageId { get; init; }
    public string? Email { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllNewsletterSubscriberResponse
{
    public int Id { get; init; }
    public string Email { get; init; } = default!;
    public int LanguageId { get; init; }
    public DateTime SubscribedAtUtc { get; init; }
    public bool IsActive { get; init; }
}
