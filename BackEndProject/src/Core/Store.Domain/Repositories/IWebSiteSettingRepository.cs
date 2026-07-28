using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IWebSiteSettingRepository
{
    ValueTask<WebSiteSetting?> GetAsync();
    Task<WebSiteSetting?> GetAsNoTrackingAsync(CancellationToken cancellationToken = default);
}