using Store.Domain.Dtos.NewsletterSubscribers;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface INewsletterSubscriberRepository
{
    Task<PagedResult<GetAllNewsletterSubscriberResponseDto>> GetAllAsync(GetAllNewsletterSubscriberRequestDto request);
    Task<NewsletterSubscriber?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    void Add(NewsletterSubscriber entity);
}
