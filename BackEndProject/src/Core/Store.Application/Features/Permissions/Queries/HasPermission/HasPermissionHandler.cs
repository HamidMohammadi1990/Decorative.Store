using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Permissions.Queries;

public class HasPermissionHandler
	(IPermissionRepository permissionRepository, IPermissionMapperService mapper)
	: IRequestHandler<HasPermissionRequest, OperationResult<HasPermissionResponse>>
{
	public async Task<OperationResult<HasPermissionResponse>> Handle(HasPermissionRequest request, CancellationToken cancellationToken)
	{
		var hasPermission = await permissionRepository.HasPermissionAsync(request.UserId, request.PermissionType);
		return mapper.Map(hasPermission);
	}
}