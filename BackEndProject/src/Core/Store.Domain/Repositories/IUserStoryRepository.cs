using Store.Domain.Dtos.UserStories;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IUserStoryRepository
{
    void Add(UserStory userStory);
    void Remove(UserStory userStory);
    Task<UserStory?> FindByIdAndUserIdAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<List<UserStoryListDto>> GetByUserIdAsync(int userId, int languageId, int defaultLanguageId, CancellationToken cancellationToken = default);
    Task<List<UserStoryListDto>> GetActiveAsync(int languageId, int defaultLanguageId, int limit, CancellationToken cancellationToken = default);
}
