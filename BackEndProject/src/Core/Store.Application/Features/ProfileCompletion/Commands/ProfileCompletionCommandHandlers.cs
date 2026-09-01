using System.Text.Json;
using Edition.Application.Contracts;
using Edition.Application.Contracts.Persistence;
using Edition.Application.Features.ProfileCompletion.Models;
using Store.Common.Models;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProfileCompletion.Commands;

public record UpdateProfileCompletionConfigRequest : IRequest<OperationResult>
{
    public string ConfigJson { get; init; } = default!;
}

public class UpdateProfileCompletionConfigHandler
    (IUnitOfWork uow, IProfileCompletionRepository repository)
    : IRequestHandler<UpdateProfileCompletionConfigRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        UpdateProfileCompletionConfigRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ConfigJson))
            return ErrorModel.Create("ValidationError");

        try
        {
            JsonDocument.Parse(request.ConfigJson);
        }
        catch (JsonException)
        {
            return ErrorModel.Create("ValidationError");
        }

        var setting = await repository.GetSettingAsync(cancellationToken);
        if (setting is null)
        {
            repository.AddSetting(ProfileCompletionSetting.Create(request.ConfigJson));
        }
        else
        {
            setting.UpdateConfig(request.ConfigJson);
        }

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        return saveResult.IsSuccess ? OperationResult.Success() : saveResult;
    }
}

public record SaveProfileCompletionAnswersRequest : IRequest<OperationResult>
{
    public string AnswersJson { get; init; } = default!;
}

public class SaveProfileCompletionAnswersHandler
    (IUnitOfWork uow, IProfileCompletionRepository repository, ICurrentUserContext currentUser)
    : IRequestHandler<SaveProfileCompletionAnswersRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        SaveProfileCompletionAnswersRequest request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId <= 0)
            return ErrorModel.Create("Unauthorized");

        if (string.IsNullOrWhiteSpace(request.AnswersJson))
            return ErrorModel.Create("ValidationError");

        try
        {
            JsonDocument.Parse(request.AnswersJson);
        }
        catch (JsonException)
        {
            return ErrorModel.Create("ValidationError");
        }

        var state = await repository.GetUserStateAsync(currentUser.UserId, cancellationToken);
        if (state is null)
        {
            repository.AddUserState(ProfileCompletionUserState.Create(currentUser.UserId, request.AnswersJson));
        }
        else
        {
            state.UpdateAnswers(request.AnswersJson);
        }

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        return saveResult.IsSuccess ? OperationResult.Success() : saveResult;
    }
}

public record ClaimProfileCompletionRewardRequest : IRequest<OperationResult<ClaimProfileCompletionRewardResponse>>
{
}

public record ClaimProfileCompletionRewardResponse
{
    public DateTime ClaimedOnUtc { get; init; }
}

public class ClaimProfileCompletionRewardHandler
    (IUnitOfWork uow, IProfileCompletionRepository repository, ICurrentUserContext currentUser)
    : IRequestHandler<ClaimProfileCompletionRewardRequest, OperationResult<ClaimProfileCompletionRewardResponse>>
{
    public async Task<OperationResult<ClaimProfileCompletionRewardResponse>> Handle(
        ClaimProfileCompletionRewardRequest request,
        CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated || currentUser.UserId <= 0)
            return ErrorModel.Create("Unauthorized");

        var state = await repository.GetUserStateAsync(currentUser.UserId, cancellationToken);
        if (state is null)
            return ErrorModel.Create("ValidationError");

        if (state.RewardClaimedOnUtc.HasValue)
            return ErrorModel.Create("DuplicateRecord");

        state.ClaimReward();

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveResult.IsSuccess)
            return saveResult.ToGenericFailure<ClaimProfileCompletionRewardResponse>();

        return new ClaimProfileCompletionRewardResponse
        {
            ClaimedOnUtc = state.RewardClaimedOnUtc!.Value,
        };
    }
}
