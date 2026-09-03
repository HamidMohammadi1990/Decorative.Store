using Store.Domain.Dtos.Seo;

namespace Store.Domain.Repositories;

public interface ISeoRepository
{
    Task<IReadOnlyList<SitemapUrlDto>> GetPublicSitemapUrlsAsync(CancellationToken cancellationToken = default);
}
