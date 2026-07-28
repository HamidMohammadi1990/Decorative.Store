using Edition.Application.Models.Dtos;
using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Users.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Users;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class UserMapperService : IUserMapperService
{
    public GetAllUserRequestDto Map(GetAllUserRequest model)
    {
        return new GetAllUserRequestDto
        {
            Email = model.Email,
            CityId = model.CityId,
            Gender = model.Gender,
            IsActive = model.IsActive,
            LastName = model.LastName,
            UserName = model.UserName,
            FirstName = model.FirstName,
            Pagination = model.Pagination,
            PhoneNumber = model.PhoneNumber,
            EconomicCode = model.EconomicCode,
            RefundMethod = model.RefundMethod,
            LoginPermission = model.LoginPermission,
            EmailConfirmed = model.EmailConfirmed,
            PhoneNumberConfirmed = model.PhoneNumberConfirmed
        }.WithContentPolicy<User, GetAllUserRequestDto>(model);
    }

    public GetUserResponse Map(User model)
    {
        return new GetUserResponse
        {
            Id = model.Id,
            Email = model.Email,
            Gender = model.Gender,
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber
        };
    }

    public PagedResult<GetAllUserResponse> Map(PagedResult<GetAllUserDto> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllUserResponse
            {
                Id = x.Id,
                Email = x.Email,
                CityId = x.CityId,
                CityName = x.CityName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                UserName = x.UserName,
                IsActive = x.IsActive,
                EconomicCode = x.EconomicCode,
                PhoneNumber = x.PhoneNumber,
                RefundMethod = x.RefundMethod,
                EmailConfirmed = x.EmailConfirmed,
                LoginPermission = x.LoginPermission,
                AccessFailedCount = x.AccessFailedCount,
                LastLoginDateOnUtc = x.LastLoginDateOnUtc,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed
            })
            .ToList();

        return PagedResult<GetAllUserResponse>.Create(items, model);
    }

    public GetForgetPasswordOptionResponse Map(string userName, List<ForgetPasswordOptionDto> options)
    {
        return new GetForgetPasswordOptionResponse
        {
            Options = options,
            Username = userName
        };
    }

    public UserNameCheckResponse Map(bool hasAccount, bool isPhoneNumber)
    {
        return new UserNameCheckResponse
        {
            HasAccount = hasAccount,
            IsPhoneNumber = isPhoneNumber
        };
    }
}