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
        var propertyCategory = await propertyCategoryRepository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (propertyCategory is null)
            return ErrorModel.Create("InvalidId");

        propertyCategory.Update(request.Code, request.IsActive, request.LanguageId, request.Title);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
