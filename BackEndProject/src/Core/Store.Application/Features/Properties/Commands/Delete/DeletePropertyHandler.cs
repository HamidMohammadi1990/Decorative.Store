using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Properties.Commands;

public class DeletePropertyHandler
    (IUnitOfWork uow, IPropertyRepository propertyRepository)
    : IRequestHandler<DeletePropertyRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeletePropertyRequest request, CancellationToken cancellationToken)
    {
        var propertyCategory = await propertyRepository.FindAsync(request.Id);
        if (propertyCategory is null)
            return ErrorModel.Create("InvalidId");

        propertyCategory.DeActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}