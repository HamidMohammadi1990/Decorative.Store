using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Cities.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Cities;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CityMapperService : ICityMapperService
{
    public PagedResult<GetAllCityResponse> Map(PagedResult<GetAllCityResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCityResponse
            {
                Id = x.Id,
                Rate = x.Rate,
                Slug = x.Slug,
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ProvinceId = x.ProvinceId,
                ProvinceName = x.ProvinceName,
                Description = x.Description
            })
            .ToList();

        return PagedResult<GetAllCityResponse>.Create(items, model);
    }

    public PagedResult<SearchCityResponse> Map(PagedResult<SearchCityResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchCityResponse
            {
                Id = x.Id,
                Rate = x.Rate,
                Slug = x.Slug,
                Name = x.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ProvinceId = x.ProvinceId,
                ProvinceName = x.ProvinceName,
                Description = x.Description
            })
            .ToList();

        return PagedResult<SearchCityResponse>.Create(items, model);
    }

    public GetCityResponse Map(City model)
    {
        return new GetCityResponse
        {
            Id = model.Id,
            Name = model.Name,
            Slug = model.Slug,
            Rate = model.Rate,
            IsActive = model.IsActive,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            ProvinceId = model.ProvinceId,
            Description = model.Description
        };
    }

    public GetAllCityRequestDto Map(GetAllCityRequest model)
    {
        return new GetAllCityRequestDto
        {
            Name = model.Name,
            Slug = model.Slug,
            IsActive = !model.IsActive,
            Pagination = model.Pagination,
            ProvinceId = model.ProvinceId
        }.WithContentPolicy<City, GetAllCityRequestDto>(model);
    }

    public SearchCityRequestDto Map(SearchCityRequest model)
    {
        return new SearchCityRequestDto
        {
            Name = model.Name,
            Pagination = model.Pagination,
            ProvinceId = model.ProvinceId
        }.WithContentPolicy<City, SearchCityRequestDto>(model);
    }
}