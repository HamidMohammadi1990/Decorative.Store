using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Queries;

public class SearchLanguageHandler(
    ILanguageRepository languageRepository,
    ILanguageMapperService mapper)
    : IRequestHandler<SearchLanguageRequest, OperationResult<PagedResult<SearchLanguageResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchLanguageResponse>>> Handle(
        SearchLanguageRequest request,
        CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var languages = await languageRepository.SearchAsync(requestModel, cancellationToken);
        return mapper.MapToSearch(languages);
    }
}
