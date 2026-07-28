using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Persistence;
using Store.Common.Localization;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Commands;

public class DeleteLanguageHandler(
    IUnitOfWork uow,
    ILanguageRepository languageRepository,
    ILanguageRegistry languageRegistry)
    : IRequestHandler<DeleteLanguageRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteLanguageRequest request, CancellationToken cancellationToken)
    {
        var language = await languageRepository.FindAsync(request.Id, cancellationToken);
        if (language is null)
            return ErrorModel.Create(MessageKeys.InvalidId);

        if (language.IsDefault)
            return ErrorModel.Create(MessageKeys.InvalidRequest);

        if (await languageRepository.CountAsync(cancellationToken) <= 1)
            return ErrorModel.Create(MessageKeys.InvalidRequest);

        languageRepository.Remove(language);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        await languageRegistry.InvalidateAsync(cancellationToken);
        return OperationResult.Success();
    }
}
