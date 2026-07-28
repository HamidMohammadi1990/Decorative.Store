using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyComments.Queries;

public class GetCompanyCommentHandler
    (ICompanyCommentRepository companyCommentRepository, ICompanyCommentMapperService mapper)
    : IRequestHandler<GetCompanyCommentRequest, OperationResult<GetCompanyCommentResponse>>
{
    public async Task<OperationResult<GetCompanyCommentResponse>> Handle(GetCompanyCommentRequest request, CancellationToken cancellationToken)
    {
        var companyComment = await companyCommentRepository.GetAsNoTrackingAsync(request.Id);
        if (companyComment is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(companyComment);
        return result;
    }
}