using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public class GetCompanyStoryCommentHandler
    (ICompanyStoryCommentRepository companyStoryCommentRepository, ICompanyStoryCommentMapperService mapper)
    : IRequestHandler<GetCompanyStoryCommentRequest, OperationResult<GetCompanyStoryCommentResponse?>>
{
    public async Task<OperationResult<GetCompanyStoryCommentResponse?>> Handle(GetCompanyStoryCommentRequest request, CancellationToken cancellationToken)
    {
        var companyStoryComment = await companyStoryCommentRepository.GetAsNoTrackingAsync(request.Id, cancellationToken);
        if (companyStoryComment is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(companyStoryComment);
    }
}
