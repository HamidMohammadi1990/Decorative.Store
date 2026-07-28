using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyCategories.Commands;

public class DeletePropertyCategoryHandler
    (IUnitOfWork uow, IPropertyCategoryRepository propertyCategoryRepository)
    : IRequestHandler<DeletePropertyCategoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeletePropertyCategoryRequest request, CancellationToken cancellationToken)
    {
        var propertyCategory = await propertyCategoryRepository.FindAsync(request.Id);
        if (propertyCategory is null)
            return ErrorModel.Create("InvalidId");

        propertyCategory.DeActive();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}