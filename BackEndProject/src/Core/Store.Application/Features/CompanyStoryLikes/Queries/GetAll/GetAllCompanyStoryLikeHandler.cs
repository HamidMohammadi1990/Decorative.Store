using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStoryLikes.Queries;

public class GetAllCompanyStoryLikeHandler
    (ICompanyStoryLikeRepository companyStoryLikeRepository, ICompanyStoryLikeMapperService mapper)
    : IRequestHandler<GetAllCompanyStoryLikeRequest, OperationResult<PagedResult<GetAllCompanyStoryLikeResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCompanyStoryLikeResponse>>> Handle(GetAllCompanyStoryLikeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var likes = await companyStoryLikeRepository.GetAllAsync(requestModel);
        return mapper.Map(likes);
    }
}
