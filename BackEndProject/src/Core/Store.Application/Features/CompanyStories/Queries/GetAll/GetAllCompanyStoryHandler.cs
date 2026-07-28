using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStories.Queries;

public class GetAllCompanyStoryHandler
    (ICompanyStoryRepository companyStoryRepository, ICompanyStoryMapperService mapper)
    : IRequestHandler<GetAllCompanyStoryRequest, OperationResult<PagedResult<GetAllCompanyStoryResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCompanyStoryResponse>>> Handle(GetAllCompanyStoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var stories = await companyStoryRepository.GetAllAsync(requestModel);
        return mapper.Map(stories);
    }
}
