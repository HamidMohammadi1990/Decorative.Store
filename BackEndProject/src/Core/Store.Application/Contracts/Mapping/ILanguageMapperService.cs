using Edition.Application.Features.Languages.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.Languages;

namespace Edition.Application.Contracts.Mapping;

public interface ILanguageMapperService : IMapper
{
    GetLanguageResponse Map(Language model);
    PagedResult<GetAllLanguageResponse> Map(PagedResult<Language> languages);
    GetAllLanguageRequestDto Map(GetAllLanguageRequest model);
    SearchLanguageRequestDto Map(SearchLanguageRequest model);
    PagedResult<SearchLanguageResponse> MapToSearch(PagedResult<Language> languages);
}
