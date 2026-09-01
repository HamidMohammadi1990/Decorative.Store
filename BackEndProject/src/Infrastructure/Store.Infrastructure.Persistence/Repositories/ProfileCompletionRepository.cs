using Microsoft.EntityFrameworkCore;
using Store.Domain.Dtos.ProfileCompletion;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;
using Store.Domain.Repositories;
using Store.Infrastructure.Persistence.Extensions;

namespace Store.Infrastructure.Persistence.Repositories;

public class ProfileCompletionRepository(EditionDbContext context)
    : Repository<ProfileCompletionSetting>(context), IProfileCompletionRepository
{
    public Task<ProfileCompletionSetting?> GetSettingAsync(CancellationToken cancellationToken = default)
        => Context.ProfileCompletionSetting.FirstOrDefaultAsync(cancellationToken);

    public void AddSetting(ProfileCompletionSetting setting)
        => Context.ProfileCompletionSetting.Add(setting);

    public Task<ProfileCompletionUserState?> GetUserStateAsync(int userId, CancellationToken cancellationToken = default)
        => Context.ProfileCompletionUserState.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

    public Task<ProfileCompletionUserState?> GetUserStateByIdAsync(int id, CancellationToken cancellationToken = default)
        => Context.ProfileCompletionUserState
            .AsNoTracking()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<ProfileCompletionUserState?> GetUserStateByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
        => Context.ProfileCompletionUserState
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<PagedResult<GetAllProfileCompletionUserStateResponseDto>> GetAllUserStatesAsync(
        GetAllProfileCompletionUserStateRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var query = Context.ProfileCompletionUserState
            .AsNoTracking()
            .Include(x => x.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.UserSearch))
        {
            var term = request.UserSearch.Trim();
            query = query.Where(x =>
                (x.User.Email != null && x.User.Email.Contains(term)) ||
                x.User.UserName.Contains(term) ||
                (x.User.FirstName != null && x.User.FirstName.Contains(term)) ||
                (x.User.LastName != null && x.User.LastName.Contains(term)));
        }

        if (request.RewardClaimed.HasValue)
        {
            query = request.RewardClaimed.Value
                ? query.Where(x => x.RewardClaimedOnUtc != null)
                : query.Where(x => x.RewardClaimedOnUtc == null);
        }

        return await query
            .OrderByDescending(x => x.UpdatedOnUtc)
            .ThenByDescending(x => x.Id)
            .Select(x => new GetAllProfileCompletionUserStateResponseDto
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.User.UserName,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Email = x.User.Email,
                AnswersJson = x.AnswersJson,
                UpdatedOnUtc = x.UpdatedOnUtc,
                RewardClaimedOnUtc = x.RewardClaimedOnUtc,
            })
            .ToPagedAsync(request.Pagination);
    }

    public void AddUserState(ProfileCompletionUserState state)
        => Context.ProfileCompletionUserState.Add(state);

    public void RemoveUserState(ProfileCompletionUserState state)
        => Context.ProfileCompletionUserState.Remove(state);
}
