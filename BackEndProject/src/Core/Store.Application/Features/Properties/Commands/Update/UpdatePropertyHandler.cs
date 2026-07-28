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
        var propertyCategory = await propertyRepository.FindAsync(request.Id);
        if (propertyCategory is null)
            return ErrorModel.Create("InvalidId");

        propertyCategory
            .Update(request.PropertyType, request.ParentId,
            request.Title, request.PropertyCategoryId,
            request.Priority, request.Status);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}