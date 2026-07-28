using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserRoles.Queries;

public class GetAllUserRoleHandler
    (IUserRoleRepository userRoleRepository, IUserRoleMapperService mapper)
    : IRequestHandler<GetAllUserRoleRequest, OperationResult<PagedResult<GetAllUserRoleResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllUserRoleResponse>>> Handle(GetAllUserRoleRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var userRoles = await userRoleRepository.GetAllAsync(requestModel);
        return mapper.Map(userRoles);
    }
}
