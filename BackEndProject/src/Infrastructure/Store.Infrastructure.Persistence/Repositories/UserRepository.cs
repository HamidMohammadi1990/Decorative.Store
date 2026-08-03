using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Users;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class UserRepository
    (EditionDbContext context)
    : Repository<User>(context), IUserRepository
{
    public async Task<PagedResult<GetAllUserDto>> GetAllAsync(GetAllUserRequestDto request)
    {
        var userSource = Context.User
            .ApplyContentPolicyFilter(request.ContentFilter);

        var users =
            from user in userSource
            select user;

        users = users.ApplyQueryFilters(request);

        var result =
            await users
                .Select(x => new GetAllUserDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    Gender = x.Gender,
                    IsActive = x.IsActive,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    PhoneNumber = x.PhoneNumber,
                    RefundMethod = x.RefundMethod,
                    EconomicCode = x.EconomicCode,
                    EmailConfirmed = x.EmailConfirmed,
                    LoginPermission = x.LoginPermission,
                    AccessFailedCount = x.AccessFailedCount,
                    LastLoginDateOnUtc = x.LastLoginDateOnUtc,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed
                })
                .AsNoTracking()
                .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<User?> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await Context.User.SingleOrDefaultAsync(x => x.UserName == userName && x.IsActive, cancellationToken);
    }

    public async Task<User?> FindByLoginAsync(string userNameOrEmail, CancellationToken cancellationToken = default)
    {
        return await Context.User.FirstOrDefaultAsync(
            x => (x.UserName == userNameOrEmail
                  || x.Email == userNameOrEmail
                  || x.PhoneNumber == userNameOrEmail) && x.IsActive,
            cancellationToken);
    }

    public async Task<string?> GetSecurityStampAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.User
            .AsNoTracking()
            .Where(x => x.Id == userId && x.IsActive)
            .Select(x => x.SecurityStamp)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> VerifyRepeatPhoneAsync(string phoneNumber)
    {
        return await Context.User.AnyAsync(u => (u.PhoneNumber == phoneNumber || u.UserName == phoneNumber) && u.PhoneNumberConfirmed);
    }

    public async Task<bool> VerifyRepeatEmailAsync(string email)
    {
        return await Context.User.AnyAsync(u => (u.Email == email || u.UserName == email) && u.EmailConfirmed);
    }
}