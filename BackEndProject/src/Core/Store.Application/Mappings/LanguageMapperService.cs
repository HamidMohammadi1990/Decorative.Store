using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Languages.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Dtos.Languages;

namespace Edition.Application.Mappings;

public class LanguageMapperService : ILanguageMapperService
{
    public GetAllLanguageRequestDto Map(GetAllLanguageRequest model)
        => new()
        {
            Code = model.Code,
            Name = model.Name,
            IsActive = model.IsActive,
            IsDefault = model.IsDefault,
            Pagination = model.Pagination
        };

    public GetLanguageResponse Map(Language model)
        => new()
        {
            Id = model.Id,
            Code = model.Code,
            Name = model.Name,
            IsActive = model.IsActive,
            IsDefault = model.IsDefault,
            DisplayOrder = model.DisplayOrder,
            IsRtl = model.IsRtl
        };

    public PagedResult<GetAllLanguageResponse> Map(PagedResult<Language> model)
    {
        var items = model.Items
            .Select(x => new GetAllLanguageResponse
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                IsActive = x.IsActive,
                IsDefault = x.IsDefault,
                DisplayOrder = x.DisplayOrder,
                IsRtl = x.IsRtl
            })
            .ToList();

        return PagedResult<GetAllLanguageResponse>.Create(items, model);
    }

    public SearchLanguageRequestDto Map(SearchLanguageRequest model)
        => new()
        {
            Code = model.Code,
            Name = model.Name,
            Pagination = model.Pagination
        };

    public PagedResult<SearchLanguageResponse> MapToSearch(PagedResult<Language> model)
    {
        var items = model.Items
            .Select(x => new SearchLanguageResponse
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                IsDefault = x.IsDefault,
                DisplayOrder = x.DisplayOrder,
                IsRtl = x.IsRtl
            })
            .ToList();

        return PagedResult<SearchLanguageResponse>.Create(items, model);
    }
}
