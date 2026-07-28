using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Cities;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class CityRepository
    (EditionDbContext context)
    : Repository<City>(context), ICityRepository
{
    public async Task<PagedResult<GetAllCityResponseDto>> GetAllAsync(GetAllCityRequestDto request)
    {
        var cities = Context.City
            .Include(x => x.Province)
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await cities
            .Select(x => new GetAllCityResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Rate = x.Rate,
                Slug = x.Slug,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ProvinceId = x.ProvinceId,
                ProvinceName = x.Province.Name,
                Description = x.Description
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<SearchCityResponseDto>> SearchAsync(SearchCityRequestDto request)
    {
        var cities = Context.City
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Include(x => x.Province)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        return await cities
            .Select(x => new SearchCityResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Rate = x.Rate,
                Slug = x.Slug,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ProvinceId = x.ProvinceId,
                ProvinceName = x.Province.Name,
                Description = x.Description
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }
}