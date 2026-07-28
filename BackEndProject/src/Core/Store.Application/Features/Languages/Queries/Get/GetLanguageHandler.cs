using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Queries;

public class GetLanguageHandler(
    ILanguageRepository languageRepository,
    ILanguageMapperService mapper)
    : IRequestHandler<GetLanguageRequest, OperationResult<GetLanguageResponse?>>
{
    public async Task<OperationResult<GetLanguageResponse?>> Handle(
        GetLanguageRequest request,
        CancellationToken cancellationToken)
    {
        var language = await languageRepository.GetAsNoTrackingAsync(request.Id, cancellationToken);
        if (language is null)
            return ErrorModel.Create(MessageKeys.InvalidId);

        return mapper.Map(language);
    }
}