using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.UserStories;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Repositories;

public class UserStoryRepository
    (EditionDbContext context)
    : Repository<UserStory>(context), IUserStoryRepository
{
    public Task<UserStory?> FindByIdAndUserIdAsync(int id, int userId, CancellationToken cancellationToken = default)
        => Context.UserStory
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

    public async Task<List<UserStoryListDto>> GetByUserIdAsync(
        int userId,
        int languageId,
        int defaultLanguageId,
        CancellationToken cancellationToken = default)
    {
        return await Context.UserStory
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedOnUtc)
            .Select(x => new UserStoryListDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Caption = x.Caption,
                MediaType = x.MediaType,
                MediaPath = x.MediaPath,
                MediaAlt = x.MediaAlt,
                PosterPath = x.PosterPath,
                ProductSlug = x.ProductId == null
                    ? null
                    : x.Product!.Translations
                        .Where(t => t.LanguageId == languageId || t.LanguageId == defaultLanguageId)
                        .OrderByDescending(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault(),
                IsActive = x.IsActive,
                CreatedOnUtc = x.CreatedOnUtc,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<UserStoryListDto>> GetActiveAsync(
        int languageId,
        int defaultLanguageId,
        int limit,
        CancellationToken cancellationToken = default)
    {
        return await Context.UserStory
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedOnUtc)
            .Take(limit)
            .Select(x => new UserStoryListDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Caption = x.Caption,
                MediaType = x.MediaType,
                MediaPath = x.MediaPath,
                MediaAlt = x.MediaAlt,
                PosterPath = x.PosterPath,
                ProductSlug = x.ProductId == null
                    ? null
                    : x.Product!.Translations
                        .Where(t => t.LanguageId == languageId || t.LanguageId == defaultLanguageId)
                        .OrderByDescending(t => t.LanguageId == languageId)
                        .Select(t => t.Slug)
                        .FirstOrDefault(),
                IsActive = x.IsActive,
                CreatedOnUtc = x.CreatedOnUtc,
                OwnerFirstName = x.User.FirstName,
                OwnerLastName = x.User.LastName,
            })
            .ToListAsync(cancellationToken);
    }
}
