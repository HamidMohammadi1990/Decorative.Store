using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Commands;

public class UpdateCategoryHandler
    (IUnitOfWork uow, ICategoryRepository categoryRepository)
    : IRequestHandler<UpdateCategoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FindWithTranslationsAsync(request.Id, cancellationToken);
        if (category is null)
            return ErrorModel.Create("InvalidId");

        category.Update(request.Code, request.IsActive, request.LanguageId, request.Title, request.Slug);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
