using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Persistence;
using Store.Common.Localization;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Commands;

public class SetDefaultLanguageHandler(
    IUnitOfWork uow,
    ILanguageRepository languageRepository,
    ILanguageRegistry languageRegistry)
    : IRequestHandler<SetDefaultLanguageRequest, OperationResult>
{
    public async Task<OperationResult> Handle(SetDefaultLanguageRequest request, CancellationToken cancellationToken)
    {
        var language = await languageRepository.FindAsync(request.Id, cancellationToken);
        if (language is null)
            return ErrorModel.Create(MessageKeys.InvalidId);

        if (!language.IsActive)
            return ErrorModel.Create(MessageKeys.InvalidRequest);

        await languageRepository.ClearDefaultAsync(language.Id, cancellationToken);
        language.SetAsDefault();

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        await languageRegistry.InvalidateAsync(cancellationToken);
        return OperationResult.Success();
    }
}
