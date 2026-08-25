using System.Linq.Expressions;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProductWishlistRepository
{
    void Add(ProductWishlist productWishlist);
    void Remove(ProductWishlist productWishlist);
    Task<bool> AnyAsync(Expression<Func<ProductWishlist, bool>> expression, CancellationToken cancellationToken = default);
    Task<ProductWishlist?> FindByUserAndProductAsync(int userId, int productId, CancellationToken cancellationToken = default);
    Task<List<ProductWishlist>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<string>> GetSlugsByUserIdAsync(int userId, int languageId, int defaultLanguageId, CancellationToken cancellationToken = default);
}
