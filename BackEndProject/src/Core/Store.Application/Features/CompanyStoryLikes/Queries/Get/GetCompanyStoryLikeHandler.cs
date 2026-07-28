using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStoryLikes.Queries;

public class GetCompanyStoryLikeHandler
    (ICompanyStoryLikeRepository companyStoryLikeRepository, ICompanyStoryLikeMapperService mapper)
    : IRequestHandler<GetCompanyStoryLikeRequest, OperationResult<GetCompanyStoryLikeResponse?>>
{
    public async Task<OperationResult<GetCompanyStoryLikeResponse?>> Handle(GetCompanyStoryLikeRequest request, CancellationToken cancellationToken)
    {
        var companyStoryLike = await companyStoryLikeRepository.GetAsNoTrackingAsync(request.Id, cancellationToken);
        if (companyStoryLike is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(companyStoryLike);
    }
}
