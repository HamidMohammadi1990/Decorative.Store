using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.Sections.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.Sections;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class SectionMapperService : ISectionMapperService
{
    public PagedResult<GetAllSectionResponse> Map(PagedResult<GetAllSectionResponseDto> model)
    {
        var items = model.Items.Select(x => new GetAllSectionResponse
        {
            Id = x.Id,
            SectionTypeId = x.SectionTypeId,
            ParentId = x.ParentId,
            ImageUrl = x.ImageUrl,
            StartDateOnUtc = x.StartDateOnUtc,
            EndDateOnUtc = x.EndDateOnUtc,
            IsActive = x.IsActive,
            SectionTypeName = x.SectionTypeName,
            ParentTitle = x.ParentTitle,
            Translations = x.Translations,
        }).ToList();

        return PagedResult<GetAllSectionResponse>.Create(items, model);
    }

    public PagedResult<SearchSectionResponse> MapToSearch(PagedResult<SearchSectionResponseDto> model)
    {
        var items = model.Items.Select(x => new SearchSectionResponse
        {
            Id = x.Id,
            SectionTypeId = x.SectionTypeId,
            ParentId = x.ParentId,
            Title = x.Title,
            Description = x.Description,
            Url = x.Url,
            ImageUrl = x.ImageUrl,
            IsActive = x.IsActive,
        }).ToList();

        return PagedResult<SearchSectionResponse>.Create(items, model);
    }

    public GetSectionResponse Map(Section model, string title, string? description, string url)
    {
        return new GetSectionResponse
        {
            Id = model.Id,
            SectionTypeId = model.SectionTypeId,
            ParentId = model.ParentId,
            Title = title,
            Description = description,
            Url = url,
            ImageUrl = model.ImageUrl,
            StartDateOnUtc = model.StartDateOnUtc,
            EndDateOnUtc = model.EndDateOnUtc,
            IsActive = model.IsActive,
        };
    }

    public GetAllSectionRequestDto Map(GetAllSectionRequest model)
    {
        return new GetAllSectionRequestDto
        {
            SectionTypeId = model.SectionTypeId,
            ParentId = model.ParentId,
            Title = model.Title,
            Url = model.Url,
            IsActive = model.IsActive,
            Pagination = model.Pagination,
        }.WithContentPolicy<Section, GetAllSectionRequestDto>(model);
    }

    public SearchSectionRequestDto Map(SearchSectionRequest model)
    {
        return new SearchSectionRequestDto
        {
            SectionTypeId = model.SectionTypeId,
            ParentId = model.ParentId,
            Title = model.Title,
            Url = model.Url,
            Pagination = model.Pagination,
        }.WithContentPolicy<Section, SearchSectionRequestDto>(model);
    }
}
