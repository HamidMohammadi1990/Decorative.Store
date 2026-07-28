using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Queries;

public class GetCategoryHandler
    (ICategoryRepository categoryRepository, ICategoryMapperService mapper)
    : IRequestHandler<GetCategoryRequest, OperationResult<GetCategoryResponse?>>
{
    public async Task<OperationResult<GetCategoryResponse?>> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetAsNoTrackingAsync(request.Id);
        if (category is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(category);
        return result;
    }
}