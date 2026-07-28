using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Queries;

public class SearchSectionTypeHandler
    (ISectionTypeRepository repository, ISectionTypeMapperService mapper)
    : IRequestHandler<SearchSectionTypeRequest, OperationResult<PagedResult<SearchSectionTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchSectionTypeResponse>>> Handle(SearchSectionTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var models = await repository.SearchAsync(requestModel);
        return mapper.MapToSearch(models);
    }
}
