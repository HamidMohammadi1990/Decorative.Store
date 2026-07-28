using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionItems.Queries;

public class SearchSectionItemHandler
    (ISectionItemRepository repository, ISectionItemMapperService mapper)
    : IRequestHandler<SearchSectionItemRequest, OperationResult<PagedResult<SearchSectionItemResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchSectionItemResponse>>> Handle(SearchSectionItemRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var models = await repository.SearchAsync(requestModel);
        return mapper.MapToSearch(models);
    }
}
