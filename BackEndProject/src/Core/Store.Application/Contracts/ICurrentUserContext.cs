using Edition.Application.Models.Services;

namespace Edition.Application.Contracts;

public interface ICurrentUserContext
{
    int UserId { get; }

    Guid? SessionId { get; }

    bool IsAuthenticated { get; }

    bool IsCooperation { get; }

    string? ClientIp { get; }

    UserSessionContext GetSessionContext();
}
