using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Queries;

public class GetPageBySlugHandler
    (IPageRepository pageRepository)
    : IRequestHandler<GetPageBySlugRequest, OperationResult<GetPageBySlugResponse>>
{
    public async Task<OperationResult<GetPageBySlugResponse>> Handle(GetPageBySlugRequest request, CancellationToken cancellationToken)
    {
        var page = await pageRepository.GetActiveBySlugWithContentAsync(request.Slug, cancellationToken);
        if (page is null)
            return ErrorModel.Create("PageNotFound");

        var utcNow = DateTime.UtcNow;

        var sections = page.PageSections
            .Where(ps => ps.Section.IsVisibleAt(utcNow, ps.Section.SectionType.IsActive))
            .OrderBy(ps => ps.Priority)
            .Select(ps => new PageSectionRenderResponse
            {
                SectionTypeId = ps.Section.SectionTypeId,
                Priority = ps.Priority,
                Title = ps.Section.Title,
                Description = ps.Section.Description,
                Url = ps.Section.Url,
                ImageUrl = ps.Section.ImageUrl,
                Items = ps.Section.SectionItems
                    .Where(i => i.IsActive)
                    .OrderBy(i => i.Priority)
                    .Select(i => new SectionItemRenderResponse
                    {
                        Title = i.Title,
                        Priority = i.Priority,
                        Icon = i.Icon,
                        ImageUrl = i.ImageUrl,
                        Url = i.Url,
                        Description = i.Description
                    })
                    .ToList()
            })
            .ToList();

        return new GetPageBySlugResponse
        {
            Slug = page.Slug,
            Title = page.Title,
            Type = page.Type,
            MetaTitle = page.MetaTitle,
            MetaDescription = page.MetaDescription,
            Sections = sections
        };
    }
}
