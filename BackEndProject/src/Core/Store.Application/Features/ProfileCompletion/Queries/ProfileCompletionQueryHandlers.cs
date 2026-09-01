using Edition.Application.Contracts;
using Edition.Application.Features.ProfileCompletion.Models;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProfileCompletion.Queries;

public record GetProfileCompletionConfigRequest : IRequest<OperationResult<GetProfileCompletionConfigResponse>>
{
}

public record GetProfileCompletionConfigResponse
{
    public string ConfigJson { get; init; } = default!;
}

public class GetProfileCompletionConfigHandler
    (IProfileCompletionRepository repository)
    : IRequestHandler<GetProfileCompletionConfigRequest, OperationResult<GetProfileCompletionConfigResponse>>
{
    public async Task<OperationResult<GetProfileCompletionConfigResponse>> Handle(
        GetProfileCompletionConfigRequest request,
        CancellationToken cancellationToken)
    {
        var setting = await repository.GetSettingAsync(cancellationToken);
        return new GetProfileCompletionConfigResponse
        {
            ConfigJson = setting?.ConfigJson ?? ProfileCompletionDefaults.ConfigJson,
        };
    }
}

public record GetMyProfileCompletionStateRequest : IRequest<OperationResult<GetMyProfileCompletionStateResponse>>
{
}

public record GetMyProfileCompletionStateResponse
{
    public string AnswersJson { get; init; } = "{}";
    public DateTime? RewardClaimedOnUtc { get; init; }
}

public class GetMyProfileCompletionStateHandler
    (IProfileCompletionRepository repository, ICurrentUserContext currentUser)
    : IRequestHandler<GetMyProfileCompletionStateRequest, OperationResult<GetMyProfileCompletionStateResponse>>
{
    public async Task<OperationResult<GetMyProfileCompletionStateResponse>> Handle(
        GetMyProfileCompletionStateRequest request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId <= 0)
            return ErrorModel.Create("Unauthorized");

        var state = await repository.GetUserStateAsync(currentUser.UserId, cancellationToken);
        return new GetMyProfileCompletionStateResponse
        {
            AnswersJson = state?.AnswersJson ?? "{}",
            RewardClaimedOnUtc = state?.RewardClaimedOnUtc,
        };
    }
}
