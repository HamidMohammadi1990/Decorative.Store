using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Roles.Commands;

public class UpdateRoleHandler
    (IUnitOfWork uow, IRoleRepository roleRepository)
    : IRequestHandler<UpdateRoleRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FindAsync(request.Id);
        if (role is null)
            return ErrorModel.Create("InvalidId");

        role.Update(request.Title, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}