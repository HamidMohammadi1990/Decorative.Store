using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.Roles.Queries;

public class GetAllRoleHandler
    (IRoleRepository roleRepository, IRoleMapperService mapper)
    : IRequestHandler<GetAllRoleRequest, OperationResult<PagedResult<GetAllRoleResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllRoleResponse>>> Handle(GetAllRoleRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var roles = await roleRepository.GetAllAsync(requestModel);
        var result = mapper.Map(roles);
        return result;
    }
}
