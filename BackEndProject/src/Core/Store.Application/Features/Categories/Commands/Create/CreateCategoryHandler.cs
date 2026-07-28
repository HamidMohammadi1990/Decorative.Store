using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.Categories.Commands;

public class CreateCategoryHandler
    (IUnitOfWork uow, ICategoryRepository categoryRepository)
    : IRequestHandler<CreateCategoryRequest, OperationResult<CreateCategoryResponse>>
{
    public async Task<OperationResult<CreateCategoryResponse>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = Category.Create(request.Title, request.Slug, request.Code);
        categoryRepository.Add(category);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCategoryResponse>();

        return new CreateCategoryResponse { Id = category.Id };
    }
}