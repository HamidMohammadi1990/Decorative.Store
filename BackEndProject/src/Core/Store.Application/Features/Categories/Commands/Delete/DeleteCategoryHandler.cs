using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Commands;

public class DeleteCategoryHandler
    (IUnitOfWork uow, ICategoryRepository categoryRepository)
    : IRequestHandler<DeleteCategoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FindAsync(request.Id);
        if (category is null)
            return ErrorModel.Create("InvalidId");

        categoryRepository.Remove(category);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
