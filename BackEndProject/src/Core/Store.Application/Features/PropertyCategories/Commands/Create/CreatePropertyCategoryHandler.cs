using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.PropertyCategories.Commands;

public class CreatePropertyCategoryHandler
    (IUnitOfWork uow, IPropertyCategoryRepository propertyCategoryRepository)
    : IRequestHandler<CreatePropertyCategoryRequest, OperationResult<CreatePropertyCategoryResponse>>
{
    public async Task<OperationResult<CreatePropertyCategoryResponse>> Handle(CreatePropertyCategoryRequest request, CancellationToken cancellationToken)
    {
        var propertyCategory = PropertyCategory.Create(request.Title);
        propertyCategoryRepository.Add(propertyCategory);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreatePropertyCategoryResponse>();

        return new CreatePropertyCategoryResponse { Id = propertyCategory.Id };
    }
}