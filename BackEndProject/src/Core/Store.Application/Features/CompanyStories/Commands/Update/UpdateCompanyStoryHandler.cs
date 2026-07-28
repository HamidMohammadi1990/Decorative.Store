using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStories.Commands;

public class UpdateCompanyStoryHandler
    (IUnitOfWork uow, ICompanyStoryRepository companyStoryRepository)
    : IRequestHandler<UpdateCompanyStoryRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateCompanyStoryRequest request, CancellationToken cancellationToken)
    {
        var companyStory = await companyStoryRepository.FindWithItemsAsync(request.Id, cancellationToken);
        if (companyStory is null)
            return ErrorModel.Create("InvalidId");

        companyStory.Update(request.Caption, request.ExpiresAtUtc);

        var items = request.Items
            .Select(x => CompanyStoryItem.Create(companyStory.Id, x.MediaType, x.FileName, x.Priority, x.DurationSeconds))
            .ToList();

        companyStory.ReplaceItems(items);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
