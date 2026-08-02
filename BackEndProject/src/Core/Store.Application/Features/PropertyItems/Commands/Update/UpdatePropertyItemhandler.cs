using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItems.Commands;

public class UpdatePropertyItemhandler
    (IUnitOfWork uow, IPropertyItemRepository propertyItemrepository)
    : IRequestHandler<UpdatePropertyItemRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdatePropertyItemRequest request, CancellationToken cancellationToken)
    {
        var propertyItem = await propertyItemrepository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (propertyItem is null)
            return ErrorModel.Create("InvalidId");

        propertyItem.Update(
            request.Code,
            request.PropertyId,
            request.Status,
            request.Priority,
            request.LanguageId,
            request.Title);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
