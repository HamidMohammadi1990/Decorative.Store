using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStories.Queries;

public class SearchCompanyStoryHandler
    (ICompanyStoryRepository companyStoryRepository, ICompanyStoryMapperService mapper)
    : IRequestHandler<SearchCompanyStoryRequest, OperationResult<PagedResult<SearchCompanyStoryResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchCompanyStoryResponse>>> Handle(SearchCompanyStoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var stories = await companyStoryRepository.SearchAsync(requestModel);
        return mapper.Map(stories);
    }
}
