using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.Seo;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class SeoRepository(EditionDbContext context) : ISeoRepository
{
    public async Task<IReadOnlyList<SitemapUrlDto>> GetPublicSitemapUrlsAsync(
        CancellationToken cancellationToken = default)
    {
        var productUrls = await context.Product
            .AsNoTracking()
            .Where(x => x.IsActive)
            .SelectMany(x => x.Translations, (product, translation) => new SitemapUrlDto
            {
                Path = $"/product/{translation.Slug}",
                LastModifiedUtc = product.CreatedOnUtc,
                ChangeFrequency = "weekly",
                Priority = 0.8m,
            })
            .ToListAsync(cancellationToken);

        var blogUrls = await context.BlogPost
            .AsNoTracking()
            .Where(x => x.IsActive && x.IsPublished)
            .SelectMany(x => x.Translations, (post, translation) => new SitemapUrlDto
            {
                Path = $"/blog/{translation.Slug}",
                LastModifiedUtc = post.UpdatedOnUtc ?? post.PublishedOnUtc ?? post.CreatedOnUtc,
                ChangeFrequency = "monthly",
                Priority = 0.7m,
            })
            .ToListAsync(cancellationToken);

        var cmsUrls = await context.Page
            .AsNoTracking()
            .Where(x => x.IsActive)
            .SelectMany(x => x.Translations, (_, translation) => new SitemapUrlDto
            {
                Path = $"/p/{translation.Slug}",
                ChangeFrequency = "monthly",
                Priority = 0.5m,
            })
            .Where(x => !string.IsNullOrWhiteSpace(x.Path) && x.Path != "/p/")
            .ToListAsync(cancellationToken);

        return productUrls
            .Concat(blogUrls)
            .Concat(cmsUrls)
            .GroupBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .OrderBy(x => x.Path)
            .ToList();
    }
}
