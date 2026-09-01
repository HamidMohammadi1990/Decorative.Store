using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.NewsletterSubscribers;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class NewsletterSubscriberRepository
    (EditionDbContext context)
    : Repository<NewsletterSubscriber>(context), INewsletterSubscriberRepository
{
    public async Task<PagedResult<GetAllNewsletterSubscriberResponseDto>> GetAllAsync(
        GetAllNewsletterSubscriberRequestDto request)
    {
        return await Context.NewsletterSubscriber
            .ApplyQueryFilters(request)
            .OrderByDescending(x => x.SubscribedAtUtc)
            .ThenByDescending(x => x.Id)
            .AsNoTracking()
            .Select(x => new GetAllNewsletterSubscriberResponseDto
            {
                Id = x.Id,
                Email = x.Email,
                LanguageId = x.LanguageId,
                SubscribedAtUtc = x.SubscribedAtUtc,
                IsActive = x.IsActive,
            })
            .ToPagedAsync(request.Pagination);
    }

    public Task<NewsletterSubscriber?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => Context.NewsletterSubscriber
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
}
