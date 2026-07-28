using Edition.Application.Models.Dtos;
using Edition.Application.Features.Users.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Users;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IUserMapperService : IMapper
{
    GetUserResponse Map(User model);
    PagedResult<GetAllUserResponse> Map(PagedResult<GetAllUserDto> model);
    GetAllUserRequestDto Map(GetAllUserRequest model);
    GetForgetPasswordOptionResponse Map(string userName, List<ForgetPasswordOptionDto> options);
    UserNameCheckResponse Map(bool hasAccount, bool isPhoneNumber);
}