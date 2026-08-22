using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProductWishlistRepository
    (EditionDbContext context)
    : Repository<ProductWishlist>(context), IProductWishlistRepository
{
    public Task<ProductWishlist?> FindByUserAndProductAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
        => Context.ProductWishlist
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId, cancellationToken);

    public async Task<List<string>> GetSlugsByUserIdAsync(
        int userId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken = default)
    {
        var items = await Context.ProductWishlist
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedOnUtc)
            .Select(x => new
            {
                x.ProductId,
                Slug = x.Product.Translations
                    .Where(t => t.LanguageId == languageId || t.LanguageId == defaultLanguageId)
                    .OrderByDescending(t => t.LanguageId == languageId)
                    .Select(t => t.Slug)
                    .FirstOrDefault(),
            })
            .ToListAsync(cancellationToken);

        return items
            .Select(x => x.Slug)
            .Where(slug => !string.IsNullOrWhiteSpace(slug))
            .Select(slug => slug!)
            .ToList();
    }

    public new Task<bool> AnyAsync(
        Expression<Func<ProductWishlist, bool>> expression,
        CancellationToken cancellationToken = default)
        => base.AnyAsync(expression, cancellationToken);
}
