using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Properties.Commands;

public class UpdatePropertyHandler
    (IUnitOfWork uow, IPropertyRepository propertyRepository)
    : IRequestHandler<UpdatePropertyRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdatePropertyRequest request, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (property is null)
            return ErrorModel.Create("InvalidId");

        property.Update(
            request.PropertyType,
            request.ParentId,
            request.Code,
            request.PropertyCategoryId,
            request.Priority,
            request.Status,
            request.LanguageId,
            request.Title,
            request.Description);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
