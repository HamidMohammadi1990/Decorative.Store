using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Permissions.Queries;

public record HasPermissionRequest : IRequest<OperationResult<HasPermissionResponse>>
{
    public int UserId { get; init; }
    public PermissionType PermissionType { get; init; }
}