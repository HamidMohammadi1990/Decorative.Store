using Edition.Application.Features.Roles.Queries;
using Store.Domain.Dtos.Roles;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IRoleMapperService : IMapper
{
    GetRoleResponse Map(Role model);
    PagedResult<GetAllRoleResponse> Map(PagedResult<Role> model);
    GetAllRoleRequestDto Map(GetAllRoleRequest model);
}