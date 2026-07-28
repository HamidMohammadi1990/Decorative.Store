using Edition.Application.Contracts;
using Store.Common.Models;

namespace Edition.Application.Features.UserSessions.Queries;

public class GetActiveUserSessionsHandler
    (ICurrentUserContext currentUser, IUserSessionService userSessionService)
    : IRequestHandler<GetActiveUserSessionsRequest, OperationResult<List<GetActiveUserSessionResponse>>>
{
    public async Task<OperationResult<List<GetActiveUserSessionResponse>>> Handle(GetActiveUserSessionsRequest request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var currentSessionId = currentUser.SessionId;
        var sessions = await userSessionService.GetActiveSessionsAsync(userId, cancellationToken);

        var result = sessions.Select(session => new GetActiveUserSessionResponse
        {
            Id = session.Id,
            DeviceName = session.DeviceName ?? "دستگاه نامشخص",
            DeviceType = session.DeviceType,
            OperatingSystem = session.OperatingSystem,
            IpAddress = session.IpAddress,
            CreatedOnUtc = session.CreatedOnUtc,
            LastSeenOnUtc = session.LastSeenOnUtc,
            ExpiresOnUtc = session.ExpiresOnUtc,
            IsCurrent = currentSessionId.HasValue && session.Id == currentSessionId.Value
        }).ToList();

        return result;
    }
}