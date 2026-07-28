using Edition.Application.Contracts.Localization;
using Edition.Application.Contracts.Persistence;
using Store.Common.Localization;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Commands;

public class CreateLanguageHandler(
    IUnitOfWork uow,
    ILanguageRepository languageRepository,
    ILanguageRegistry languageRegistry)
    : IRequestHandler<CreateLanguageRequest, OperationResult<CreateLanguageResponse>>
{
    public async Task<OperationResult<CreateLanguageResponse>> Handle(
        CreateLanguageRequest request,
        CancellationToken cancellationToken)
    {
        var code = LanguageCultureNormalizer.Normalize(request.Code);

        var language = Language.Create(
            code,
            request.Name,
            request.IsActive,
            isDefault: false,
            request.DisplayOrder,
            request.IsRtl);

        languageRepository.Add(language);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateLanguageResponse>();

        await languageRegistry.InvalidateAsync(cancellationToken);

        return new CreateLanguageResponse { Id = language.Id };
    }
}
