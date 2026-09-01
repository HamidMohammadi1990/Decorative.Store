using Store.Domain.Dtos.Pagination;

namespace Store.Domain.Dtos.ProfileCompletion;

public record GetAllProfileCompletionUserStateRequestDto
{
    public string? UserSearch { get; init; }
    public bool? RewardClaimed { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllProfileCompletionUserStateResponseDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string UserName { get; init; } = default!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string AnswersJson { get; init; } = default!;
    public DateTime UpdatedOnUtc { get; init; }
    public DateTime? RewardClaimedOnUtc { get; init; }
}
