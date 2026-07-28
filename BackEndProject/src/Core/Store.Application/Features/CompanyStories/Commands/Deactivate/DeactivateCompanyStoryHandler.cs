using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.CompanyStories.Commands;

public class DeactivateCompanyStoryHandler
    (IUnitOfWork uow, ICompanyStoryRepository companyStoryRepository)
    : IRequestHandler<DeactivateCompanyStoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeactivateCompanyStoryRequest request, CancellationToken cancellationToken)
    {
        var companyStory = await companyStoryRepository.FindAsync(request.Id, cancellationToken);
        if (companyStory is null)
            return ErrorModel.Create("InvalidId");

        companyStory.Deactivate();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
