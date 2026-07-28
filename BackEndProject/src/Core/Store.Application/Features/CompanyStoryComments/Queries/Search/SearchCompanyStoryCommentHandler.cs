using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public class SearchCompanyStoryCommentHandler
    (ICompanyStoryCommentRepository companyStoryCommentRepository, ICompanyStoryCommentMapperService mapper)
    : IRequestHandler<SearchCompanyStoryCommentRequest, OperationResult<PagedResult<SearchCompanyStoryCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchCompanyStoryCommentResponse>>> Handle(SearchCompanyStoryCommentRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var comments = await companyStoryCommentRepository.SearchAsync(requestModel);
        return mapper.Map(comments);
    }
}
