using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItems.Commands;

public class DeletePropertyItemHandler
    (IUnitOfWork uow, IPropertyItemRepository propertyItemrepository)
    : IRequestHandler<DeletePropertyItemRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeletePropertyItemRequest request, CancellationToken cancellationToken)
    {
        var propertyItem = await propertyItemrepository.FindAsync(request.Id);
        if (propertyItem is null)
            return ErrorModel.Create("InvalidId");

        propertyItem.DeActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}