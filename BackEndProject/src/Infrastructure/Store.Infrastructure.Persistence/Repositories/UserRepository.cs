using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
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
            join city in Context.City on user.CityId equals city.Id into joinCity
            from city in joinCity.DefaultIfEmpty()
            select new { user, city };

        users = users.ApplyQueryFilters(request);

        var result =
            await users
                .Select(x => new GetAllUserDto
                {
                    Id = x.user.Id,
                    Email = x.user.Email,
                    CityId = x.user.CityId,
                    Gender = x.user.Gender,
                    IsActive = x.user.IsActive,
                    CityName = x.city.Name,
                    FirstName = x.user.FirstName,
                    LastName = x.user.LastName,
                    UserName = x.user.UserName,
                    PhoneNumber = x.user.PhoneNumber,
                    RefundMethod = x.user.RefundMethod,
                    EconomicCode = x.user.EconomicCode,
                    EmailConfirmed = x.user.EmailConfirmed,
                    LoginPermission = x.user.LoginPermission,
                    AccessFailedCount = x.user.AccessFailedCount,
                    LastLoginDateOnUtc = x.user.LastLoginDateOnUtc,
                    PhoneNumberConfirmed = x.user.PhoneNumberConfirmed
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
            x => (x.UserName == userNameOrEmail || x.Email == userNameOrEmail) && x.IsActive,
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