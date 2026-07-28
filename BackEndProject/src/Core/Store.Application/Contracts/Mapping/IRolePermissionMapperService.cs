using Edition.Application.Features.RolePermissions.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.RolePermissions;

namespace Edition.Application.Contracts.Mapping;

public interface IRolePermissionMapperService : IMapper
{
    GetAllRolePermissionRequestDto Map(GetAllRolePermissionRequest model);
    GetRolePermissionResponse Map(GetRolePermissionDto model);
    PagedResult<GetAllRolePermissionResponse> Map(PagedResult<GetAllRolePermissionDto> model);
}
