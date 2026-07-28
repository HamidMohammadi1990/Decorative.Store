using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.UserRoles.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.UserRoles;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class UserRoleMapperService : IUserRoleMapperService
{
    public GetAllUserRoleRequestDto Map(GetAllUserRoleRequest model)
    {
        return new GetAllUserRoleRequestDto
        {
            UserId = model.UserId,
            RoleId = model.RoleId,
            Pagination = model.Pagination
        }.WithContentPolicy<UserRole, GetAllUserRoleRequestDto>(model);
    }

    public GetUserRoleResponse Map(GetUserRoleDto model)
    {
        return new GetUserRoleResponse
        {
            Id = model.Id,
            UserId = model.UserId,
            UserName = model.UserName,
            RoleId = model.RoleId,
            RoleTitle = model.RoleTitle
        };
    }

    public PagedResult<GetAllUserRoleResponse> Map(PagedResult<GetAllUserRoleDto> model)
    {
        var items = model.Items.Select(x => new GetAllUserRoleResponse
        {
            Id = x.Id,
            UserId = x.UserId,
            UserName = x.UserName,
            RoleId = x.RoleId,
            RoleTitle = x.RoleTitle
        }).ToList();

        return PagedResult<GetAllUserRoleResponse>.Create(items, model);
    }
}
