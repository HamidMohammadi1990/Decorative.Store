using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStories.Commands;

public class ActivateCompanyStoryHandler
    (IUnitOfWork uow, ICompanyStoryRepository companyStoryRepository)
    : IRequestHandler<ActivateCompanyStoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(ActivateCompanyStoryRequest request, CancellationToken cancellationToken)
    {
        var companyStory = await companyStoryRepository.FindAsync(request.Id, cancellationToken);
        if (companyStory is null)
            return ErrorModel.Create("InvalidId");

        companyStory.Activate();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
