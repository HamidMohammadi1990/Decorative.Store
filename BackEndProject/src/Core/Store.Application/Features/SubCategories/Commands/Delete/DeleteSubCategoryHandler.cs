using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SubCategories.Commands;

public class DeleteSubCategoryHandler
    (IUnitOfWork uow, ISubCategoryRepository subCategoryRepository)
    : IRequestHandler<DeleteSubCategoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var subCategory = await subCategoryRepository.FindAsync(request.Id);
        if (subCategory is null)
            return ErrorModel.Create("InvalidId");

        subCategoryRepository.Remove(subCategory);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}