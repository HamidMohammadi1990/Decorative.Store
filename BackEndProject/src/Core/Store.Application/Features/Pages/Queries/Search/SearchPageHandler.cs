using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Queries;

public class SearchPageHandler
    (IPageRepository pageRepository, IPageMapperService mapper)
    : IRequestHandler<SearchPageRequest, OperationResult<PagedResult<SearchPageResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchPageResponse>>> Handle(SearchPageRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var pages = await pageRepository.SearchAsync(requestModel);
        return mapper.MapToSearch(pages);
    }
}
