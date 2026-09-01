using Store.Domain.Dtos.Pagination;
using Store.Domain.Dtos.ProfileCompletion;
using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IProfileCompletionRepository
{
    Task<ProfileCompletionSetting?> GetSettingAsync(CancellationToken cancellationToken = default);
    void AddSetting(ProfileCompletionSetting setting);
    Task<ProfileCompletionUserState?> GetUserStateAsync(int userId, CancellationToken cancellationToken = default);
    Task<ProfileCompletionUserState?> GetUserStateByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProfileCompletionUserState?> GetUserStateByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<GetAllProfileCompletionUserStateResponseDto>> GetAllUserStatesAsync(
        GetAllProfileCompletionUserStateRequestDto request,
        CancellationToken cancellationToken = default);
    void AddUserState(ProfileCompletionUserState state);
    void RemoveUserState(ProfileCompletionUserState state);
}
