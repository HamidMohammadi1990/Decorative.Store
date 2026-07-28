using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Sections.Queries;

public class SearchSectionHandler
    (ISectionRepository repository, ISectionMapperService mapper)
    : IRequestHandler<SearchSectionRequest, OperationResult<PagedResult<SearchSectionResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchSectionResponse>>> Handle(SearchSectionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var models = await repository.SearchAsync(requestModel);
        return mapper.MapToSearch(models);
    }
}
