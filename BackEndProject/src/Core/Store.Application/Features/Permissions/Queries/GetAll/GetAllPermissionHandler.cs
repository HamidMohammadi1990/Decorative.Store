using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Permissions.Queries;

public class GetAllPermissionHandler
    (IPermissionRepository permissionRepository, IPermissionMapperService mapper)
    : IRequestHandler<GetAllPermissionRequest, OperationResult<PagedResult<GetAllPermissionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllPermissionResponse>>> Handle(GetAllPermissionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var permissions = await permissionRepository.GetAllAsync(requestModel);
        var result = mapper.Map(permissions);
        return result;
    }
}