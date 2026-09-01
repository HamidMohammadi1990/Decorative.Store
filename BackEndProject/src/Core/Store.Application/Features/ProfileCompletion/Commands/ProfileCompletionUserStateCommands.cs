using System.Text.Json;
using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProfileCompletion.Commands;

public record DeleteProfileCompletionUserStateRequest : IRequest<OperationResult>
{
    public int Id { get; init; }
}

public class DeleteProfileCompletionUserStateHandler
    (IUnitOfWork uow, IProfileCompletionRepository repository)
    : IRequestHandler<DeleteProfileCompletionUserStateRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        DeleteProfileCompletionUserStateRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            return ErrorModel.Create("InvalidId");

        var entity = await repository.GetUserStateByIdForUpdateAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        repository.RemoveUserState(entity);

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        return saveResult.IsSuccess ? OperationResult.Success() : saveResult;
    }
}

public record UpdateProfileCompletionUserStateRequest : IRequest<OperationResult>
{
    public int Id { get; init; }
    public string? AnswersJson { get; init; }
    public bool ClearRewardClaim { get; init; }
}

public class UpdateProfileCompletionUserStateHandler
    (IUnitOfWork uow, IProfileCompletionRepository repository)
    : IRequestHandler<UpdateProfileCompletionUserStateRequest, OperationResult>
{
    public async Task<OperationResult> Handle(
        UpdateProfileCompletionUserStateRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            return ErrorModel.Create("InvalidId");

        if (request.AnswersJson is not null)
        {
            try
            {
                JsonDocument.Parse(request.AnswersJson);
            }
            catch (JsonException)
            {
                return ErrorModel.Create("ValidationError");
            }
        }

        var entity = await repository.GetUserStateByIdForUpdateAsync(request.Id, cancellationToken);
        if (entity is null)
            return ErrorModel.Create("InvalidId");

        if (request.AnswersJson is not null)
            entity.UpdateAnswers(request.AnswersJson);

        if (request.ClearRewardClaim)
            entity.ClearRewardClaim();

        var saveResult = await uow.SaveChangesAsync(cancellationToken);
        return saveResult.IsSuccess ? OperationResult.Success() : saveResult;
    }
}
