using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStories.Queries;

public class GetCompanyStoryHandler
    (ICompanyStoryRepository companyStoryRepository, ICompanyStoryMapperService mapper)
    : IRequestHandler<GetCompanyStoryRequest, OperationResult<GetCompanyStoryResponse?>>
{
    public async Task<OperationResult<GetCompanyStoryResponse?>> Handle(GetCompanyStoryRequest request, CancellationToken cancellationToken)
    {
        var companyStory = await companyStoryRepository.GetWithItemsAsNoTrackingAsync(request.Id, cancellationToken);
        if (companyStory is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(companyStory);
    }
}
