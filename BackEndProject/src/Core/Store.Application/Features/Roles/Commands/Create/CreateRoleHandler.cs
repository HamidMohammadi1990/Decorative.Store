using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Roles.Commands;

public class CreateRoleHandler
    (IUnitOfWork uow, IRoleRepository roleRepository)
    : IRequestHandler<CreateRoleRequest, OperationResult<CreateRoleResponse>>
{
    public async Task<OperationResult<CreateRoleResponse>> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = Role.Create(request.Title);
        roleRepository.Add(role);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateRoleResponse>();

        return new CreateRoleResponse { Id = role.Id };
    }
}