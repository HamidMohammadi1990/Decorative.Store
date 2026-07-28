using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStories.Commands;

public class CreateCompanyStoryHandler
    (IUnitOfWork uow, ICompanyStoryRepository companyStoryRepository, ICompanyRepository companyRepository, ICurrentUserContext currentUser)
    : IRequestHandler<CreateCompanyStoryRequest, OperationResult<CreateCompanyStoryResponse>>
{
    public async Task<OperationResult<CreateCompanyStoryResponse>> Handle(CreateCompanyStoryRequest request, CancellationToken cancellationToken)
    {
        var companyExists = await companyRepository.AnyAsync(x => x.Id == request.CompanyId, cancellationToken);
        if (!companyExists)
            return ErrorModel.Create("InvalidId");

        var userId = currentUser.UserId;
        var companyStory = CompanyStory.Create(request.CompanyId, userId, request.Caption, request.ExpiresAtUtc);

        var items = request.Items
            .Select(x => CompanyStoryItem.Create(0, x.MediaType, x.FileName, x.Priority, x.DurationSeconds))
            .ToArray();

        companyStory.AddItems(items);
        companyStoryRepository.Add(companyStory);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateCompanyStoryResponse>();

        return new CreateCompanyStoryResponse { Id = companyStory.Id };
    }
}
