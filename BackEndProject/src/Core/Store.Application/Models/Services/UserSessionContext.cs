using Store.Domain.Enums;

namespace Edition.Application.Models.Services;

public record UserSessionContext(
    string? IpAddress,
    string? UserAgent,
    string? DeviceName,
    DeviceType DeviceType,
    OperatingSystemType OperatingSystem);
