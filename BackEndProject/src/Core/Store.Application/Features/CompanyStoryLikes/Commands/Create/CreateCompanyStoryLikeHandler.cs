using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStoryLikes.Commands;

public class CreateCompanyStoryLikeHandler
    (IUnitOfWork uow, ICompanyStoryLikeRepository companyStoryLikeRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateCompanyStoryLikeRequest, OperationResult<CreateCompanyStoryLikeResponse>>
{
    public async Task<OperationResult<CreateCompanyStoryLikeResponse>> Handle(CreateCompanyStoryLikeRequest request, CancellationToken cancellationToken)
    {
        int? userId = null;
        if (currentUser.IsAuthenticated)
            userId = currentUser.UserId;

        var userIP = currentUser.ClientIp;

        var isExistsLike = await
            companyStoryLikeRepository
            .AnyAsync(x => x.CompanyStoryId == request.CompanyStoryId && (x.UserId == userId || x.ClientIP == userIP));

        if (isExistsLike)
            return ErrorModel.Create("DuplicateLike");

        var companyStoryLike = CompanyStoryLike.Create(request.CompanyStoryId, userIP, userId);
        companyStoryLikeRepository.Add(companyStoryLike);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCompanyStoryLikeResponse>();

        return new CreateCompanyStoryLikeResponse { Id = companyStoryLike.Id };
    }
}
