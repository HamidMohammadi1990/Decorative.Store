using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.UserAddresses.Queries;
using Store.Domain.Dtos.UserAddresses;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class UserAddressMapperService : IUserAddressMapperService
{
    public GetUserAddressResponse Map(UserAddress model)
    {
        return new GetUserAddressResponse
        {
            Id = model.Id,
            Title = model.Title,
            UserId = model.UserId,
            CityId = model.CityId,
            Address = model.Address,
            Apartment = model.Apartment,
            PostalCode = model.PostalCode,
            PhoneNumber = model.PhoneNumber,
            RecipientFirstName = model.RecipientFirstName,
            RecipientLastName = model.RecipientLastName,
            Latitude = model.Lat,
            Longitude = model.Long,
            IsDefault = model.IsDefault
        };
    }

    public GetUserAddressesRequestDto Map(GetUserAddressesRequest model, int userId)
    {
        return new GetUserAddressesRequestDto
        {
            Title = model.Title,
            CityId = model.CityId,
            UserId = userId,
            IsActive = model.IsActive,
            PostalCode = model.PostalCode,
            Pagination = model.Pagination
        }.WithContentPolicy<UserAddress, GetUserAddressesRequestDto>(model);
    }

    public GetAllUserAddressRequestDto Map(GetAllUserAddressRequest model)
    {
        return new GetAllUserAddressRequestDto
        {
            Title = model.Title,
            UserId = model.UserId,
            CityId = model.CityId,
            IsActive = model.IsActive,
            PostalCode = model.PostalCode,
            Pagination = model.Pagination,
        }.WithContentPolicy<UserAddress, GetAllUserAddressRequestDto>(model);
    }

    public PagedResult<GetAllUserAddressResponse> Map(PagedResult<GetAllUserAddressDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllUserAddressResponse
            {
                Id = x.Id,
                Title = x.Title,
                UserId = x.UserId,
                CityId = x.CityId,
                ProvinceId = x.ProvinceId,
                Address = x.Address,
                Apartment = x.Apartment,
                CityName = x.CityName,
                UserName = x.UserName,
                UserFirstName = x.UserFirstName,
                UserLastName = x.UserLastName,
                PostalCode = x.PostalCode,
                PhoneNumber = x.PhoneNumber,
                RecipientLastName = x.RecipientLastName,
                RecipientFirstName = x.RecipientFirstName,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsDefault = x.IsDefault
            })
            .ToList();

        return PagedResult<GetAllUserAddressResponse>.Create(items, model);
    }

    public PagedResult<GetUserAddressesResponse> Map(PagedResult<GetUserAddressDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetUserAddressesResponse
            {
                Id = x.Id,
                Title = x.Title,
                UserId = x.UserId,
                CityId = x.CityId,
                ProvinceId = x.ProvinceId,
                Address = x.Address,
                Apartment = x.Apartment,
                CityName = x.CityName,
                PostalCode = x.PostalCode,
                PhoneNumber = x.PhoneNumber,
                RecipientLastName = x.RecipientLastName,
                RecipientFirstName = x.RecipientFirstName,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                IsDefault = x.IsDefault
            })
            .ToList();

        return PagedResult<GetUserAddressesResponse>.Create(items, model);
    }
}
