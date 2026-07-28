using Edition.Application.Mappings;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyComments.Queries;

public class GetAllCompanyCommentHandler
    (ICompanyCommentRepository companyCommentRepository, CompanyCommentMapperService mapper)
    : IRequestHandler<GetAllCompanyCommentRequest, OperationResult<PagedResult<GetAllCompanyCommentResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllCompanyCommentResponse>>> Handle(GetAllCompanyCommentRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var comments = await companyCommentRepository.GetAllAsync(requestModel);
        var result = mapper.Map(comments);
        return result;
    }
}