using Store.Common.Models;

namespace Edition.Application.Features.Roles.Commands;

public record CreateRoleRequest : IRequest<OperationResult<CreateRoleResponse>>
{
    public string Title { get; init; } = default!;
}