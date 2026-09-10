using Edition.Application.Common.Extensions;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.PageSections.Queries;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.PageSections;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class PageSectionMapperService : IPageSectionMapperService
{
    public PagedResult<GetAllPageSectionResponse> Map(PagedResult<GetAllPageSectionResponseDto> model)
    {
        var items = model.Items.Select(x => new GetAllPageSectionResponse
        {
            Id = x.Id,
            PageId = x.PageId,
            SectionId = x.SectionId,
            Priority = x.Priority,
            AdminDescription = x.AdminDescription,
            PageTitle = x.PageTitle,
            PageSlug = x.PageSlug,
            SectionTitle = x.SectionTitle,
            SectionTypeName = x.SectionTypeName,
        }).ToList();

        return PagedResult<GetAllPageSectionResponse>.Create(items, model);
    }

    public PagedResult<SearchPageSectionResponse> MapToSearch(PagedResult<PageSection> model)
    {
        var items = model.Items.Select(x => new SearchPageSectionResponse
        {
            Id = x.Id,
            PageId = x.PageId,
            SectionId = x.SectionId,
            Priority = x.Priority
        }).ToList();

        return PagedResult<SearchPageSectionResponse>.Create(items, model);
    }

    public GetPageSectionResponse Map(PageSection model)
    {
        return new GetPageSectionResponse
        {
            Id = model.Id,
            PageId = model.PageId,
            SectionId = model.SectionId,
            Priority = model.Priority
        };
    }

    public GetAllPageSectionRequestDto Map(GetAllPageSectionRequest model)
    {
        return new GetAllPageSectionRequestDto
        {
            PageId = model.PageId,
            SectionId = model.SectionId,
            Pagination = model.Pagination
        }.WithContentPolicy<PageSection, GetAllPageSectionRequestDto>(model);
    }

    public SearchPageSectionRequestDto Map(SearchPageSectionRequest model)
    {
        return new SearchPageSectionRequestDto
        {
            PageId = model.PageId,
            SectionId = model.SectionId,
            Pagination = model.Pagination
        }.WithContentPolicy<PageSection, SearchPageSectionRequestDto>(model);
    }
}
