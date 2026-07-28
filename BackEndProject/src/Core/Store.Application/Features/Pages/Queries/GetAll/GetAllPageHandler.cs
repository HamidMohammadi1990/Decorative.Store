using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Queries;

public class GetAllPageHandler
    (IPageRepository pageRepository, IPageMapperService mapper)
    : IRequestHandler<GetAllPageRequest, OperationResult<PagedResult<GetAllPageResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllPageResponse>>> Handle(GetAllPageRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var pages = await pageRepository.GetAllAsync(requestModel);
        return mapper.Map(pages);
    }
}
