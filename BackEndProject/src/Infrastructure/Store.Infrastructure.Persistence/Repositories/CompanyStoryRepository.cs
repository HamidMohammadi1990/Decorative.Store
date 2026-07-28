using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Persistence.Extensions;
using Store.Infrastructure.Persistence;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.CompanyStories;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class CompanyStoryRepository
    (EditionDbContext context)
    : Repository<CompanyStory>(context), ICompanyStoryRepository
{
    public async ValueTask<CompanyStory?> FindWithItemsAsync(int id, CancellationToken cancellationToken = default)
        => await Context.CompanyStory
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<CompanyStory?> GetWithItemsAsNoTrackingAsync(int id, CancellationToken cancellationToken = default)
        => await Context.CompanyStory
            .AsNoTracking()
            .Include(x => x.Items.OrderBy(i => i.Priority))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<GetAllCompanyStoryDto>> GetAllAsync(GetAllCompanyStoryRequestDto request)
    {
        var companyStories = Context.CompanyStory
            .ApplyContentPolicyFilter(request.ContentFilter);

        var stories =
            from companyStory in companyStories
            join company in Context.Company on companyStory.CompanyId equals company.Id
            join user in Context.User on companyStory.CreatedByUserId equals user.Id
            select new { companyStory, company, user };

        stories = stories.ApplyQueryFilters(request);

        var result = await stories
            .Select(x => new GetAllCompanyStoryDto
            {
                Id = x.companyStory.Id,
                Caption = x.companyStory.Caption,
                CompanyId = x.companyStory.CompanyId,
                CompanyName = x.company.Name,
                IsActive = x.companyStory.IsActive,
                CreatedOnUtc = x.companyStory.CreatedOnUtc,
                UpdatedOnUtc = x.companyStory.UpdatedOnUtc,
                ExpiresAtUtc = x.companyStory.ExpiresAtUtc,
                CreatedByUserId = x.companyStory.CreatedByUserId,
                CreatedByUserFirstName = x.user.FirstName!,
                CreatedByUserLastName = x.user.LastName!,
                LikeCount = x.companyStory.Likes.Count,
                CommentCount = x.companyStory.Comments.Count,
                Items = x.companyStory.Items
                    .OrderBy(i => i.Priority)
                    .Select(i => new CompanyStoryItemDto
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        Priority = i.Priority,
                        MediaType = i.MediaType,
                        DurationSeconds = i.DurationSeconds
                    })
                    .ToList()
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }

    public async Task<PagedResult<SearchCompanyStoryDto>> SearchAsync(SearchCompanyStoryRequestDto request)
    {
        var utcNow = DateTime.UtcNow;
        var companyStories = Context.CompanyStory
            .ApplyContentPolicyFilter(request.ContentFilter);

        var stories =
            from companyStory in companyStories
            join company in Context.Company on companyStory.CompanyId equals company.Id
            join user in Context.User on companyStory.CreatedByUserId equals user.Id
            where companyStory.IsActive && (companyStory.ExpiresAtUtc == null || companyStory.ExpiresAtUtc > utcNow)
            select new { companyStory, company, user };

        stories = stories.ApplyQueryFilters(request);

        var result = await stories
            .Select(x => new SearchCompanyStoryDto
            {
                Id = x.companyStory.Id,
                Caption = x.companyStory.Caption,
                CompanyId = x.companyStory.CompanyId,
                CompanyName = x.company.Name,
                CreatedOnUtc = x.companyStory.CreatedOnUtc,
                ExpiresAtUtc = x.companyStory.ExpiresAtUtc,
                CreatedByUserId = x.companyStory.CreatedByUserId,
                CreatedByUserFirstName = x.user.FirstName!,
                CreatedByUserLastName = x.user.LastName!,
                LikeCount = x.companyStory.Likes.Count,
                CommentCount = x.companyStory.Comments.Count,
                Items = x.companyStory.Items
                    .OrderBy(i => i.Priority)
                    .Select(i => new CompanyStoryItemDto
                    {
                        Id = i.Id,
                        FileName = i.FileName,
                        Priority = i.Priority,
                        MediaType = i.MediaType,
                        DurationSeconds = i.DurationSeconds
                    })
                    .ToList()
            })
            .AsNoTracking()
            .ToPagedAsync(request.Pagination);

        return result;
    }
}
