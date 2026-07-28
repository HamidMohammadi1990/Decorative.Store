using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SubCategories.Queries;

public class SearchSubCategoryHandler
    (ISubCategoryRepository subCategoryRepository, ISubCategoryMapperService mapper)
    : IRequestHandler<SearchSubCategoryRequest, OperationResult<PagedResult<SearchSubCategoryResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchSubCategoryResponse>>> Handle(SearchSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var subCategories = await subCategoryRepository.SearchAsync(requestModel);
        var result = mapper.Map(subCategories);
        return result;
    }
}