using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.RolePermissions.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.RolePermissions;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class RolePermissionMapperService : IRolePermissionMapperService
{
    public GetAllRolePermissionRequestDto Map(GetAllRolePermissionRequest model)
    {
        return new GetAllRolePermissionRequestDto
        {
            RoleId = model.RoleId,
            PermissionId = model.PermissionId,
            Pagination = model.Pagination
        }.WithContentPolicy<RolePermission, GetAllRolePermissionRequestDto>(model);
    }

    public GetRolePermissionResponse Map(GetRolePermissionDto model)
    {
        return new GetRolePermissionResponse
        {
            Id = model.Id,
            RoleId = model.RoleId,
            RoleTitle = model.RoleTitle,
            PermissionId = model.PermissionId,
            PermissionTitle = model.PermissionTitle
        };
    }

    public PagedResult<GetAllRolePermissionResponse> Map(PagedResult<GetAllRolePermissionDto> model)
    {
        var items = model.Items.Select(x => new GetAllRolePermissionResponse
        {
            Id = x.Id,
            RoleId = x.RoleId,
            RoleTitle = x.RoleTitle,
            PermissionId = x.PermissionId,
            PermissionTitle = x.PermissionTitle
        }).ToList();

        return PagedResult<GetAllRolePermissionResponse>.Create(items, model);
    }
}
