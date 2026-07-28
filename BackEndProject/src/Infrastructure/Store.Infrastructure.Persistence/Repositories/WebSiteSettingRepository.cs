using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence;

namespace Store.Infrastructure.Persistence.Repositories;

public class WebSiteSettingRepository
    (EditionDbContext context)
    : Repository<WebSiteSetting>(context), IWebSiteSettingRepository
{
    public async ValueTask<WebSiteSetting?> GetAsync()
    {
        return await Context.WebSiteSetting.FirstOrDefaultAsync();
    }

    public async Task<WebSiteSetting?> GetAsNoTrackingAsync(CancellationToken cancellationToken = default)
    {
        return await Context.WebSiteSetting.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
    }
}