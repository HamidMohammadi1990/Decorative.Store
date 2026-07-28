using Edition.Application.Features.UserRoles.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.UserRoles;

namespace Edition.Application.Contracts.Mapping;

public interface IUserRoleMapperService : IMapper
{
    GetAllUserRoleRequestDto Map(GetAllUserRoleRequest model);
    GetUserRoleResponse Map(GetUserRoleDto model);
    PagedResult<GetAllUserRoleResponse> Map(PagedResult<GetAllUserRoleDto> model);
}
