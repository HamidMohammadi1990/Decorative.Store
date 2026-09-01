using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.MarketingPromos;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class MarketingPromoRepository
    (EditionDbContext context)
    : Repository<MarketingPromo>(context), IMarketingPromoRepository
{
    public async Task<PagedResult<GetAllMarketingPromoResponseDto>> GetAllAsync(GetAllMarketingPromoRequestDto request)
    {
        var source = Context.MarketingPromo
            .ApplyContentPolicyFilter(request.ContentFilter);

        return await source
            .ApplyQueryFilters(request)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.Id)
            .AsNoTracking()
            .Select(x => new GetAllMarketingPromoResponseDto
            {
                Id = x.Id,
                LanguageId = x.LanguageId,
                PromoType = x.PromoType,
                Title = x.Title,
                Subtitle = x.Subtitle,
                LinkLabel = x.LinkLabel,
                LinkHref = x.LinkHref,
                ImageFileName = x.ImageFileName,
                Priority = x.Priority,
                IsActive = x.IsActive,
            })
            .ToPagedAsync(request.Pagination);
    }

    public async Task<IReadOnlyList<MarketingPromo>> GetActiveStripAsync(int languageId, CancellationToken cancellationToken = default)
        => await Context.MarketingPromo
            .Where(x => x.LanguageId == languageId && x.IsActive)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public Task<MarketingStripDisclaimer?> GetDisclaimerAsync(int languageId, CancellationToken cancellationToken = default)
        => Context.MarketingStripDisclaimer
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LanguageId == languageId, cancellationToken);

    public Task<MarketingStripDisclaimer?> GetDisclaimerForUpdateAsync(int languageId, CancellationToken cancellationToken = default)
        => Context.MarketingStripDisclaimer
            .FirstOrDefaultAsync(x => x.LanguageId == languageId, cancellationToken);

    public void AddDisclaimer(MarketingStripDisclaimer entity)
        => Context.MarketingStripDisclaimer.Add(entity);
}
