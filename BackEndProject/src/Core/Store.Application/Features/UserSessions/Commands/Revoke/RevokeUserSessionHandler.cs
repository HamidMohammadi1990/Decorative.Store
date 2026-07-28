using Edition.Application.Contracts;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.UserSessions.Commands;

public record RevokeUserSessionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(UserSessionEncryptor))]
    public Guid SessionId { get; init; }
}

public class RevokeUserSessionHandler
    (ICurrentUserContext currentUser, IUserSessionService userSessionService)
    : IRequestHandler<RevokeUserSessionRequest, OperationResult>
{
    public async Task<OperationResult> Handle(RevokeUserSessionRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var currentSessionId = currentUser.SessionId;

        if (currentSessionId.HasValue && currentSessionId.Value == request.SessionId)
            return ErrorModel.Create("CannotRevokeCurrentSession");

        var activeSessions = await userSessionService.GetActiveSessionsAsync(userId, cancellationToken);
        if (activeSessions.All(x => x.Id != request.SessionId))
            return ErrorModel.Create("InvalidSessionId");

        await userSessionService.RevokeSessionAsync(request.SessionId, userId, UserSessionRevokeReason.RevokedByUser, cancellationToken);
        return OperationResult.Success();
    }
}

public record RevokeOtherUserSessionsRequest : IRequest<OperationResult>;

public class RevokeOtherUserSessionsHandler
    (ICurrentUserContext currentUser, IUserSessionService userSessionService)
    : IRequestHandler<RevokeOtherUserSessionsRequest, OperationResult>
{
    public async Task<OperationResult> Handle(RevokeOtherUserSessionsRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var currentSessionId = currentUser.SessionId;
        if (!currentSessionId.HasValue)
            return ErrorModel.Create("SessionExpired");

        await userSessionService.RevokeAllSessionsAsync(
            userId,
            UserSessionRevokeReason.RevokedOtherSessions,
            currentSessionId,
            cancellationToken);

        return OperationResult.Success();
    }
}