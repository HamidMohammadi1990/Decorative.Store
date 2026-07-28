using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserRoles.Queries;

public class GetUserRoleHandler
    (IUserRoleRepository userRoleRepository, IUserRoleMapperService mapper)
    : IRequestHandler<GetUserRoleRequest, OperationResult<GetUserRoleResponse?>>
{
    public async Task<OperationResult<GetUserRoleResponse?>> Handle(GetUserRoleRequest request, CancellationToken cancellationToken)
    {
        var userRole = await userRoleRepository.GetInfoAsync(request.Id);
        if (userRole is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(userRole);
    }
}