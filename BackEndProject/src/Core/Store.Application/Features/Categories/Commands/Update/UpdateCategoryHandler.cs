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
        var category = await categoryRepository.FindAsync(request.Id);
        if (category is null)
            return ErrorModel.Create("InvalidId");

        category.Update(request.Title, request.Slug, request.Code, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
