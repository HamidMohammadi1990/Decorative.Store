using System.Linq.Expressions;
using Store.Domain.Dtos.UserAddresses;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IUserAddressRepository
{
    Task<PagedResult<GetAllUserAddressDto>> GetAllAsync(GetAllUserAddressRequestDto model);
    Task<PagedResult<GetUserAddressDto>> GetUserAddressAsync(GetUserAddressesRequestDto request);

    ValueTask<UserAddress?> FindAsync(int id, CancellationToken cancellationToken = default);
    Task<UserAddress?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<List<UserAddressSummaryDto>> GetSummariesAsync(int userId);
    Task<bool> AnyDefaultAsync(int userId, CancellationToken cancellationToken = default);
    Task ClearDefaultForUserAsync(int userId, int exceptAddressId, CancellationToken cancellationToken = default);
    Task PromoteNextDefaultAsync(int userId, int? exceptAddressId = null, CancellationToken cancellationToken = default);
    void Add(UserAddress userAddress);
    void Remove(UserAddress userAddress);
    Task<bool> AnyAsync(Expression<Func<UserAddress, bool>> expression, CancellationToken cancellationToken = default);
}
