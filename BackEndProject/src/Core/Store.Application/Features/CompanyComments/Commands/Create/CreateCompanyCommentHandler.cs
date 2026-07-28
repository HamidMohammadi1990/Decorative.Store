using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyComments.Commands;

public class CreateCompanyCommentHandler
    (ICompanyCommentRepository companyCommentRepository, IUnitOfWork uow, ICurrentUserContext currentUser)
    : IRequestHandler<CreateCompanyCommentRequest, OperationResult<CreateCompanyCommentResponse>>
{
    public async Task<OperationResult<CreateCompanyCommentResponse>> Handle(CreateCompanyCommentRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var companyComment = CompanyComment.Create(request.ParentId, userId, request.CompanyId, request.Title, request.Description, request.Rate);

        companyCommentRepository.Add(companyComment);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCompanyCommentResponse>();

        return new CreateCompanyCommentResponse { Id = companyComment.Id };
    }
}