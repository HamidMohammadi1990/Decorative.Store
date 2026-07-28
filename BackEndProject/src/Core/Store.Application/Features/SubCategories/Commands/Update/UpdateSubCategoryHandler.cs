using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SubCategories.Commands;

public class UpdateSubCategoryHandler
    (IUnitOfWork uow, ISubCategoryRepository subCategoryRepository)
    : IRequestHandler<UpdateSubCategoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var subCategory = await subCategoryRepository.FindAsync(request.Id);
        if (subCategory is null)
            return ErrorModel.Create("InvalidId");

        subCategory.Update(request.Title, request.Slug, request.Code, request.CategoryId, request.IsActive);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}