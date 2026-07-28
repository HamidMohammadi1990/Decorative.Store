using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.PageSections.Queries;

public class SearchPageSectionHandler
    (IPageSectionRepository repository, IPageSectionMapperService mapper)
    : IRequestHandler<SearchPageSectionRequest, OperationResult<PagedResult<SearchPageSectionResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchPageSectionResponse>>> Handle(SearchPageSectionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var models = await repository.SearchAsync(requestModel);
        return mapper.MapToSearch(models);
    }
}
