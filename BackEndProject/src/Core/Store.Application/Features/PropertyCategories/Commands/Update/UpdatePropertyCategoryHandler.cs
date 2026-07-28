using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyCategories.Commands;

public class UpdatePropertyCategoryHandler
    (IUnitOfWork uow, IPropertyCategoryRepository propertyCategoryRepository)
    : IRequestHandler<UpdatePropertyCategoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdatePropertyCategoryRequest request, CancellationToken cancellationToken)
    {
        var productDescription = await propertyCategoryRepository.FindAsync(request.Id);
        if (productDescription is null)
            return ErrorModel.Create("InvalidId");

        productDescription.Update(request.Title);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}