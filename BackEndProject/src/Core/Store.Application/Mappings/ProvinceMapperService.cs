using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Provinces.Queries;
using Store.Domain.Dtos.Provinces;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProvinceMapperService : IProvinceMapperService
{
    public GetProvinceResponse Map(Province province)
    {
        return new GetProvinceResponse
        {
            Id = province.Id,
            Name = province.Name,
            Rate = province.Rate,
            Slug = province.Slug,
            IsActive = province.IsActive,
            Latitude = province.Latitude,
            Longitude = province.Longitude,
            Description = province.Description
        };
    }

    public GetAllProvinceRequestDto Map(GetAllProvinceRequest model)
    {
        return new GetAllProvinceRequestDto
        {
            Name = model.Name,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<Province, GetAllProvinceRequestDto>(model);
    }

    public SearchProvinceRequestDto Map(SearchProvinceRequest model)
    {
        return new SearchProvinceRequestDto
        {
            Name = model.Name,
            Pagination = model.Pagination
        }.WithContentPolicy<Province, SearchProvinceRequestDto>(model);
    }

    public PagedResult<GetAllProvinceResponse> Map(PagedResult<GetAllProvinceResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllProvinceResponse
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
            .ToList();

        return PagedResult<GetAllProvinceResponse>.Create(items, model);
    }

    public PagedResult<SearchProvinceResponse> Map(PagedResult<SearchProvinceResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchProvinceResponse
            {
                Id = x.Id,
                Name = x.Name,
                Rate = x.Rate,
                Slug = x.Slug,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Description = x.Description
            })
            .ToList();

        return PagedResult<SearchProvinceResponse>.Create(items, model);
    }
}