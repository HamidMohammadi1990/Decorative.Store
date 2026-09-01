using Edition.Application.Contracts.Mapping;
using Edition.Application.Features.ProfileCompletion.Queries;
using Store.Domain.Dtos.ProfileCompletion;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Mappings;

public class ProfileCompletionUserStateMapperService : IProfileCompletionUserStateMapperService
{
    public GetAllProfileCompletionUserStateRequestDto Map(GetAllProfileCompletionUserStateRequest model)
        => new()
        {
            UserSearch = model.UserSearch,
            RewardClaimed = model.RewardClaimed,
            Pagination = model.Pagination,
        };

    public PagedResult<GetAllProfileCompletionUserStateResponse> Map(
        PagedResult<GetAllProfileCompletionUserStateResponseDto> model)
    {
        var items = model.Items.Select(MapListItem).ToList();
        return PagedResult<GetAllProfileCompletionUserStateResponse>.Create(items, model);
    }

    public GetProfileCompletionUserStateResponse Map(ProfileCompletionUserState model)
        => new()
        {
            Id = model.Id,
            UserId = model.UserId,
            UserName = model.User.UserName,
            FirstName = model.User.FirstName,
            LastName = model.User.LastName,
            Email = model.User.Email,
            AnswersJson = model.AnswersJson,
            UpdatedOnUtc = model.UpdatedOnUtc,
            RewardClaimedOnUtc = model.RewardClaimedOnUtc,
        };

    private static GetAllProfileCompletionUserStateResponse MapListItem(
        GetAllProfileCompletionUserStateResponseDto model)
        => new()
        {
            Id = model.Id,
            UserId = model.UserId,
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            AnswersJson = model.AnswersJson,
            UpdatedOnUtc = model.UpdatedOnUtc,
            RewardClaimedOnUtc = model.RewardClaimedOnUtc,
        };
}
