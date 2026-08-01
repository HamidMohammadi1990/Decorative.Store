using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.SubCategories.Commands;

public class CreateSubCategoryHandler
    (IUnitOfWork uow, ISubCategoryRepository subCategoryRepository)
    : IRequestHandler<CreateSubCategoryRequest, OperationResult<CreateSubCategoryResponse>>
{
    public async Task<OperationResult<CreateSubCategoryResponse>> Handle(CreateSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var subCategory = SubCategory.Create(request.Code, request.CategoryId);
        subCategory.UpsertTranslation(request.LanguageId, request.Title, request.Slug);
        subCategoryRepository.Add(subCategory);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateSubCategoryResponse>();

        return new CreateSubCategoryResponse { Id = subCategory.Id };
    }
}
