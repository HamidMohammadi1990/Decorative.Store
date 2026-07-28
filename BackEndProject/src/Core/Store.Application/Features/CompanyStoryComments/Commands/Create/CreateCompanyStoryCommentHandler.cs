using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public class CreateCompanyStoryCommentHandler
    (IUnitOfWork uow, ICompanyStoryCommentRepository companyStoryCommentRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateCompanyStoryCommentRequest, OperationResult<CreateCompanyStoryCommentResponse>>
{
    public async Task<OperationResult<CreateCompanyStoryCommentResponse>> Handle(CreateCompanyStoryCommentRequest request, CancellationToken cancellationToken)
    {
        int userId = currentUser.UserId;
        var companyStoryComment = CompanyStoryComment.Create(request.ParentId, request.CompanyStoryId, userId, request.Content);

        companyStoryCommentRepository.Add(companyStoryComment);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCompanyStoryCommentResponse>();

        return new CreateCompanyStoryCommentResponse { Id = companyStoryComment.Id };
    }
}
