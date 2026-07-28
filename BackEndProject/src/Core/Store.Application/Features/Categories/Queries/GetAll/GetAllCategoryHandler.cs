using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Queries;

public class GetAllCategoryHandler
    (ICategoryRepository categoryRepository, ICategoryMapperService mapper)
    : IRequestHandler<GetAllCategoryRequest, OperationResult<PagedResult<GetAllCategoryResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCategoryResponse>>> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var categories = await categoryRepository.GetAllAsync(requestModel);
        var result = mapper.Map(categories);
        return result;
    }
}