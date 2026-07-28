using Edition.Application.Features.Permissions.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Permissions;
using Store.Domain.Entities;

namespace Edition.Application.Contracts.Mapping;

public interface IPermissionMapperService : IMapper
{
    HasPermissionResponse Map(bool hasPermission);
    GetPermissionResponse Map(Permission model);
    PagedResult<GetAllPermissionResponse> Map(PagedResult<Permission> model);
    GetAllPermissionRequestDto Map(GetAllPermissionRequest model);
}