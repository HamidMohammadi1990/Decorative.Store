using Edition.Application.Contracts;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Users.Queries;

public class GetMyPermissionsHandler
    (IPermissionRepository permissionRepository, ICurrentUserContext currentUser)
    : IRequestHandler<GetMyPermissionsRequest, OperationResult<GetMyPermissionsResponse>>
{
    public async Task<OperationResult<GetMyPermissionsResponse>> Handle(
        GetMyPermissionsRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = currentUser.UserId;
        if (userId <= 0)
            return ErrorModel.Create("AccessDenied");

        var permissions = await permissionRepository.GetPermissionCodesByUserIdAsync(userId, cancellationToken);

        return new GetMyPermissionsResponse
        {
            Permissions = permissions,
        };
    }
}
