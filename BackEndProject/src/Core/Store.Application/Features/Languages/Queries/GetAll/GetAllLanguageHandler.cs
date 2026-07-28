using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Queries;

public class GetAllLanguageHandler(
    ILanguageRepository languageRepository,
    ILanguageMapperService mapper)
    : IRequestHandler<GetAllLanguageRequest, OperationResult<PagedResult<GetAllLanguageResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllLanguageResponse>>> Handle(
        GetAllLanguageRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var languages = await languageRepository.GetAllAsync(requestModel, cancellationToken);
        return mapper.Map(languages);
    }
}
