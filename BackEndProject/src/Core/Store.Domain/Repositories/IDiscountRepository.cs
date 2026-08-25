using System.Linq.Expressions;
using Store.Domain.Dtos.Discounts;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IDiscountRepository
{
    void Add(Discount discount);
    Task<bool> AnyAsync(Expression<Func<Discount, bool>> expression, CancellationToken cancellationToken = default);
    void Remove(Discount discount);
    ValueTask<Discount?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllDiscountResponseDto>> GetAllAsync(GetAllDiscountRequestDto request);
    Task<Discount?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<Discount?> GetByCodeAsync(string code);
    Task<Discount?> GetByIdAsync(int id);
    Task<List<GetAllDiscountResponseDto>> GetAvailableForUserAsync(int userId, CancellationToken cancellationToken = default);
}
