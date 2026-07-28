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
            join city in Context.City on address.CityId equals city.Id
            join user in Context.User on address.UserId equals user.Id
            select new { address, city, user };

        userAddresses = userAddresses.ApplyQueryFilters(request);

        var result = await
            userAddresses
            .Select(x => new GetAllUserAddressDto
            {
                Id = x.address.Id,
                Title = x.address.Title,
                UserId = x.address.UserId,
                CityId = x.address.CityId,
                Address = x.address.Address,
                UserName = x.user.UserName,
                UserFirstName = x.user.FirstName,
                UserLastName = x.user.LastName,
                CityName = x.city.Name,
                PostalCode = x.address.PostalCode,
                PhoneNumber = x.address.PhoneNumber,
                RecipientLastName = x.address.RecipientLastName,
                RecipientFirstName = x.address.RecipientFirstName
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
            join city in Context.City on address.CityId equals city.Id
            select new { address, city };

        userAddresses = userAddresses.ApplyQueryFilters(request);

        var result = await
            userAddresses
            .Select(x => new GetUserAddressDto
            {
                Id = x.address.Id,
                Title = x.address.Title,
                UserId = x.address.UserId,
                CityId = x.address.CityId,
                Address = x.address.Address,
                CityName = x.city.Name,
                PostalCode = x.address.PostalCode,
                PhoneNumber = x.address.PhoneNumber,
                RecipientLastName = x.address.RecipientLastName,
                RecipientFirstName = x.address.RecipientFirstName
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
            .Select(x => new UserAddressSummaryDto
            {
                Id = x.Id,
                Title = x.Title,
                Address = x.Address,
            })
            .AsNoTracking()
            .ToListAsync();
    }
}