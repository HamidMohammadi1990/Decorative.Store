using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Companies.Queries;
using Store.Domain.Dtos.Companies;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class CompanyMapperService : ICompanyMapperService
{
    public GetAllCompanyRequestDto Map(GetAllCompanyRequest model)
    {
        return new GetAllCompanyRequestDto
        {
            Code = model.Code,
            Name = model.Name,
            UserId = model.UserId,
            CityId = model.CityId,
            IsActive = model.IsActive,
            ProductId = model.ProductId,
            PostalCode = model.PostalCode,
            ProvinceId = model.ProvinceId,
            Pagination = model.Pagination
        }.WithContentPolicy<Company, GetAllCompanyRequestDto>(model);
    }

    public SearchCompanyRequestDto Map(SearchCompanyRequest model)
    {
        return new SearchCompanyRequestDto
        {
            Code = model.Code,
            Name = model.Name,
            UserId = model.UserId,
            CityId = model.CityId,
            ProductId = model.ProductId,
            PostalCode = model.PostalCode,
            ProvinceId = model.ProvinceId,
            Pagination = model.Pagination
        }.WithContentPolicy<Company, SearchCompanyRequestDto>(model);
    }

    public GetCompanyResponse Map(Company model)
    {
        return new GetCompanyResponse
        {
            Id = model.Id,
            Code = model.Code,
            Name = model.Name,
            Email = model.Email,
            UserId = model.UserId,
            CityId = model.CityId,
            Address = model.Address,
            IsActive = model.IsActive,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            PostalCode = model.PostalCode,
            PhoneNumber = model.PhoneNumber,
            Description = model.Description,
            CreatedOnUtc = model.CreatedOnUtc
        };
    }

    public PagedResult<GetAllCompanyResponse> Map(PagedResult<GetAllCompanyResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllCompanyResponse
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Email = x.Email,
                CityId = x.CityId,
                UserId = x.UserId,
                Address = x.Address,
                CityName = x.CityName,
                IsActive = x.IsActive,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ProvinceId = x.ProvinceId,
                PostalCode = x.PostalCode,
                PhoneNumber = x.PhoneNumber,
                Description = x.Description,
                ProvinceName = x.ProvinceName,
                CreatedOnUtc = x.CreatedOnUtc,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
            })
            .ToList();

        return PagedResult<GetAllCompanyResponse>.Create(items, model);
    }

    public PagedResult<SearchCompanyResponse> Map(PagedResult<SearchCompanyResponseDto> model)
    {
        var items = model
            .Items
            .Select(x => new SearchCompanyResponse
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Email = x.Email,
                CityId = x.CityId,
                UserId = x.UserId,
                Address = x.Address,
                CityName = x.CityName,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                ProvinceId = x.ProvinceId,
                PostalCode = x.PostalCode,
                PhoneNumber = x.PhoneNumber,
                Description = x.Description,
                ProvinceName = x.ProvinceName,
                CreatedOnUtc = x.CreatedOnUtc,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
            })
            .ToList();

        return PagedResult<SearchCompanyResponse>.Create(items, model);
    }
}