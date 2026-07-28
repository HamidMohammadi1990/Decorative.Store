using System.Linq.Expressions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Users;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IUserRepository
{
    Task<PagedResult<GetAllUserDto>> GetAllAsync(GetAllUserRequestDto request);
    void Add(User user);
    Task<bool> AnyAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken = default);
    ValueTask<User?> FindAsync(int userId, CancellationToken cancellationToken = default);
    Task<User?> GetAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<User?> FindByLoginAsync(string userNameOrEmail, CancellationToken cancellationToken = default);
    Task<string?> GetSecurityStampAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> VerifyRepeatPhoneAsync(string phoneNumber);
    Task<bool> VerifyRepeatEmailAsync(string email);
}