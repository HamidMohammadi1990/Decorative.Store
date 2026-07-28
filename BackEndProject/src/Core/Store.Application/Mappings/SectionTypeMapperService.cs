using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.SectionTypes.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.SectionTypes;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class SectionTypeMapperService : ISectionTypeMapperService
{
    public PagedResult<GetAllSectionTypeResponse> Map(PagedResult<SectionType> model)
    {
        var items = model.Items.Select(x => new GetAllSectionTypeResponse
        {
            Id = x.Id,
            Name = x.Name,
            IsActive = x.IsActive
        }).ToList();

        return PagedResult<GetAllSectionTypeResponse>.Create(items, model);
    }

    public PagedResult<SearchSectionTypeResponse> MapToSearch(PagedResult<SectionType> model)
    {
        var items = model.Items.Select(x => new SearchSectionTypeResponse
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        return PagedResult<SearchSectionTypeResponse>.Create(items, model);
    }

    public GetSectionTypeResponse Map(SectionType model)
    {
        return new GetSectionTypeResponse
        {
            Id = model.Id,
            Name = model.Name,
            IsActive = model.IsActive
        };
    }

    public GetAllSectionTypeRequestDto Map(GetAllSectionTypeRequest model)
    {
        return new GetAllSectionTypeRequestDto
        {
            Name = model.Name,
            IsActive = model.IsActive,
        }.WithContentPolicy<SectionType, GetAllSectionTypeRequestDto>(model);
    }

    public SearchSectionTypeRequestDto Map(SearchSectionTypeRequest model)
    {
        return new SearchSectionTypeRequestDto
        {
            Name = model.Name,
        }.WithContentPolicy<SectionType, SearchSectionTypeRequestDto>(model);
    }
}
