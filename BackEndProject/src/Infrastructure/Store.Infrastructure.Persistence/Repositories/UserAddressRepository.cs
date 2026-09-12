using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Domain.Dtos.UserAddresses;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class UserAddressRepository
    (EditionDbContext context)
    : Repository<UserAddress>(context), IUserAddressRepository
{
    public async Task<PagedResult<GetAllUserAddressDto>> GetAllAsync(GetAllUserAddressRequestDto request)
    {
        var addressSource = Context.UserAddress
            .ApplyContentPolicyFilter(request.ContentFilter);

        var userAddresses =
            from address in addressSource
            join city in Context.City on address.CityId equals city.Id into cityJoin
            from city in cityJoin.DefaultIfEmpty()
            join user in Context.User on address.UserId equals user.Id
            select new { address, city, user };

        userAddresses = userAddresses.ApplyQueryFilters(request);

        var result = await
            userAddresses
            .OrderByDescending(x => x.address.IsDefault)
            .ThenByDescending(x => x.address.Id)
            .Select(x => new GetAllUserAddressDto
            {
                Id = x.address.Id,
                Title = x.address.Title,
                UserId = x.address.UserId,
                CityId = x.address.CityId,
                ProvinceId = x.city != null ? x.city.ProvinceId : null,
                Address = x.address.Address,
                Apartment = x.address.Apartment,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                CityName = x.city != null ? x.city.Name : null,
                PostalCode = x.address.PostalCode,
                PhoneNumber = x.address.PhoneNumber,
                RecipientLastName = x.address.RecipientLastName,
                RecipientFirstName = x.address.RecipientFirstName,
                Latitude = x.address.Lat,
                Longitude = x.address.Long,
                IsDefault = x.address.IsDefault
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<GetUserAddressDto>> GetUserAddressAsync(
        GetUserAddressesRequestDto request)
    {
        var addressSource = Context.UserAddress
            .ApplyContentPolicyFilter(request.ContentFilter);

        var userAddresses =
            from address in addressSource
            join city in Context.City on address.CityId equals city.Id into cityJoin
            from city in cityJoin.DefaultIfEmpty()
            select new { address, city };

        userAddresses = userAddresses.ApplyQueryFilters(request);

        var result = await
            userAddresses
            .OrderByDescending(x => x.address.IsDefault)
            .ThenByDescending(x => x.address.Id)
            .Select(x => new GetUserAddressDto
            {
                Id = x.address.Id,
                Title = x.address.Title,
                UserId = x.address.UserId,
                CityId = x.address.CityId,
                ProvinceId = x.city != null ? x.city.ProvinceId : null,
                Address = x.address.Address,
                Apartment = x.address.Apartment,
                CityName = x.city != null ? x.city.Name : null,
                PostalCode = x.address.PostalCode,
                PhoneNumber = x.address.PhoneNumber,
                RecipientLastName = x.address.RecipientLastName,
                RecipientFirstName = x.address.RecipientFirstName,
                Latitude = x.address.Lat,
                Longitude = x.address.Long,
                IsDefault = x.address.IsDefault
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<List<UserAddressSummaryDto>> GetSummariesAsync(int userId)
    {
        return await
            Context
            .UserAddress
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.IsDefault)
            .ThenByDescending(x => x.Id)
            .Select(x => new UserAddressSummaryDto
            {
                Id = x.Id,
                Title = x.Title,
                Address = x.Address,
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<bool> AnyDefaultAsync(int userId, CancellationToken cancellationToken = default)
        => Context.UserAddress.AnyAsync(x => x.UserId == userId && x.IsDefault, cancellationToken);

    public async Task ClearDefaultForUserAsync(int userId, int exceptAddressId, CancellationToken cancellationToken = default)
    {
        var addresses = await Context.UserAddress
            .Where(x => x.UserId == userId && x.IsDefault && x.Id != exceptAddressId)
            .ToListAsync(cancellationToken);

        foreach (var address in addresses)
            address.SetDefault(false);
    }

    public async Task PromoteNextDefaultAsync(int userId, int? exceptAddressId = null, CancellationToken cancellationToken = default)
    {
        var query = Context.UserAddress
            .Where(x => x.UserId == userId && x.IsActive);

        if (exceptAddressId.HasValue)
            query = query.Where(x => x.Id != exceptAddressId.Value);

        var next = await query
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (next is not null)
        {
            next.SetDefault(true);
            return;
        }

        if (exceptAddressId.HasValue)
        {
            var fallback = await Context.UserAddress
                .FirstOrDefaultAsync(x => x.Id == exceptAddressId.Value && x.UserId == userId, cancellationToken);
            fallback?.SetDefault(true);
        }
    }
}
