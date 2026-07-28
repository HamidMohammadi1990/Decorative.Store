using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Persistence;
using Store.Common.Localization;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Commands;

public class UpdateLanguageHandler(
    IUnitOfWork uow,
    ILanguageRepository languageRepository,
    ILanguageRegistry languageRegistry)
    : IRequestHandler<UpdateLanguageRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateLanguageRequest request, CancellationToken cancellationToken)
    {
        var language = await languageRepository.FindAsync(request.Id, cancellationToken);
        if (language is null)
            return ErrorModel.Create(MessageKeys.InvalidId);

        if (language.IsDefault && !request.IsActive)
            return ErrorModel.Create(MessageKeys.InvalidRequest);

        var code = LanguageCultureNormalizer.Normalize(request.Code);
        language.Update(code, request.Name, request.IsActive, request.DisplayOrder, request.IsRtl);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        await languageRegistry.InvalidateAsync(cancellationToken);
        return OperationResult.Success();
    }
}
