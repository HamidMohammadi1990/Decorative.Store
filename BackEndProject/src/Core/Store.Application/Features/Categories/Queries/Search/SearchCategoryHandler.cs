using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Queries;

public class SearchCategoryHandler
    (ICategoryRepository categoryRepository, ICategoryMapperService mapper)
    : IRequestHandler<SearchCategoryRequest, OperationResult<PagedResult<SearchCategoryResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchCategoryResponse>>> Handle(SearchCategoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var categories = await categoryRepository.SearchAsync(requestModel, cancellationToken);
        var result = mapper.Map(categories);
        return result;
    }
}