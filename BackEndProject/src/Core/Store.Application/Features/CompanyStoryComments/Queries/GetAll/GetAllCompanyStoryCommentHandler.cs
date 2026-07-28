using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public class GetAllCompanyStoryCommentHandler
    (ICompanyStoryCommentRepository companyStoryCommentRepository, ICompanyStoryCommentMapperService mapper)
    : IRequestHandler<GetAllCompanyStoryCommentRequest, OperationResult<PagedResult<GetAllCompanyStoryCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCompanyStoryCommentResponse>>> Handle(GetAllCompanyStoryCommentRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var comments = await companyStoryCommentRepository.GetAllAsync(requestModel);
        return mapper.Map(comments);
    }
}
