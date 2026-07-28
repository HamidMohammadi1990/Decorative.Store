using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Roles.Queries;
using Store.Domain.Dtos.Roles;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class RoleMapperService : IRoleMapperService
{
    public GetAllRoleRequestDto Map(GetAllRoleRequest model)
    {
        return new GetAllRoleRequestDto
        {
            Title = model.Title,
            IsActive = model.IsActive,
            Pagination = model.Pagination
        }.WithContentPolicy<Role, GetAllRoleRequestDto>(model);
    }

    public GetRoleResponse Map(Role model)
    {
        return new GetRoleResponse
        {
            Id = model.Id,
            Title = model.Title,
            IsActive = model.IsActive
        };
    }

    public PagedResult<GetAllRoleResponse> Map(PagedResult<Role> model)
    {
        var items = model
            .Items
            .Select(x => new GetAllRoleResponse
            {
                Id = x.Id,
                Title = x.Title,
                IsActive = x.IsActive
            })
            .ToList();

        return PagedResult<GetAllRoleResponse>.Create(items, model);
    }
}