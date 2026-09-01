using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProfileCompletion.Queries;

public record GetAllProfileCompletionUserStateRequest
    : IRequest<OperationResult<PagedResult<GetAllProfileCompletionUserStateResponse>>>
{
    public string? UserSearch { get; init; }
    public bool? RewardClaimed { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}

public record GetAllProfileCompletionUserStateResponse
{
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string UserName { get; init; } = default!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string AnswersJson { get; init; } = default!;
    public DateTime UpdatedOnUtc { get; init; }
    public DateTime? RewardClaimedOnUtc { get; init; }
}

public record GetProfileCompletionUserStateRequest
    : IRequest<OperationResult<GetProfileCompletionUserStateResponse?>>
{
    public int Id { get; init; }
}

public record GetProfileCompletionUserStateResponse
{
    public int Id { get; init; }

    [JsonConverter(typeof(UserEncryptor))]
    public int UserId { get; init; }

    public string UserName { get; init; } = default!;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string AnswersJson { get; init; } = default!;
    public DateTime UpdatedOnUtc { get; init; }
    public DateTime? RewardClaimedOnUtc { get; init; }
}

public class GetAllProfileCompletionUserStateHandler
    (IProfileCompletionRepository repository, IProfileCompletionUserStateMapperService mapper)
    : IRequestHandler<GetAllProfileCompletionUserStateRequest, OperationResult<PagedResult<GetAllProfileCompletionUserStateResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProfileCompletionUserStateResponse>>> Handle(
        GetAllProfileCompletionUserStateRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetAllUserStatesAsync(mapper.Map(request), cancellationToken);
        return mapper.Map(result);
    }
}

public class GetProfileCompletionUserStateHandler
    (IProfileCompletionRepository repository, IProfileCompletionUserStateMapperService mapper)
    : IRequestHandler<GetProfileCompletionUserStateRequest, OperationResult<GetProfileCompletionUserStateResponse?>>
{
    public async Task<OperationResult<GetProfileCompletionUserStateResponse?>> Handle(
        GetProfileCompletionUserStateRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            return ErrorModel.Create("InvalidId");

        var entity = await repository.GetUserStateByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return (GetProfileCompletionUserStateResponse?)null;

        return mapper.Map(entity);
    }
}
