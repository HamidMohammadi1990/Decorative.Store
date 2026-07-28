using Edition.Application.Mappings;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyComments.Queries;

public class SearchCompanyCommentHandler
    (ICompanyCommentRepository companyCommentRepository, CompanyCommentMapperService mapper)
    : IRequestHandler<SearchCompanyCommentRequest, OperationResult<PagedResult<SearchCompanyCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchCompanyCommentResponse>>> Handle(SearchCompanyCommentRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var comments = await companyCommentRepository.SearchAsync(requestModel);
        var result = mapper.Map(comments);
        return result;
    }
}