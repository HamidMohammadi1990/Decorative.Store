using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Provinces;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProvinceRepository
    (EditionDbContext context)
    : Repository<Province>(context), IProvinceRepository
{
    public async Task<PagedResult<GetAllProvinceResponseDto>> GetAllAsync(GetAllProvinceRequestDto request)
    {
        var provinces = Context.Province
            .ApplyContentPolicyFilter(request.ContentFilter)
            .ApplyQueryFilters(request);

        return await provinces
            .Select(x => new GetAllProvinceResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Rate = x.Rate,
                Slug = x.Slug,
                IsActive = x.IsActive,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Description = x.Description
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }

    public async Task<PagedResult<SearchProvinceResponseDto>> SearchAsync(SearchProvinceRequestDto request)
    {
        var provinces = Context.Province
            .ApplyContentPolicyFilter(request.ContentFilter)
            .Where(x => x.IsActive)
            .ApplyQueryFilters(request);

        return await provinces
            .Select(x => new SearchProvinceResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Rate = x.Rate,
                Slug = x.Slug,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Description = x.Description
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);
    }
}