using System.Linq.Expressions;
using Store.Domain.Dtos.MarketingPromos;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IMarketingPromoRepository
{
    Task<PagedResult<GetAllMarketingPromoResponseDto>> GetAllAsync(GetAllMarketingPromoRequestDto request);
    Task<IReadOnlyList<MarketingPromo>> GetActiveStripAsync(int languageId, CancellationToken cancellationToken = default);
    Task<MarketingStripDisclaimer?> GetDisclaimerAsync(int languageId, CancellationToken cancellationToken = default);
    Task<MarketingStripDisclaimer?> GetDisclaimerForUpdateAsync(int languageId, CancellationToken cancellationToken = default);
    void Add(MarketingPromo entity);
    void AddDisclaimer(MarketingStripDisclaimer entity);
    Task<bool> AnyAsync(Expression<Func<MarketingPromo, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask<MarketingPromo?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<MarketingPromo?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    void Remove(MarketingPromo entity);
}
