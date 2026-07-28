using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.UserSessions.Queries;

public record GetActiveUserSessionsRequest : IRequest<OperationResult<List<GetActiveUserSessionResponse>>>;

public record GetActiveUserSessionResponse
{
    [JsonConverter(typeof(UserSessionEncryptor))]
    public Guid Id { get; init; }

    public string DeviceName { get; init; } = default!;
    public DeviceType DeviceType { get; init; }
    public OperatingSystemType OperatingSystem { get; init; }
    public string? IpAddress { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime LastSeenOnUtc { get; init; }
    public DateTime ExpiresOnUtc { get; init; }
    public bool IsCurrent { get; init; }
}
