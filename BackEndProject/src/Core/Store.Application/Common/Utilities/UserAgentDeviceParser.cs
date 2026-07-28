using Store.Domain.Enums;

namespace Edition.Application.Common.Utilities;

public static class UserAgentDeviceParser
{
    public static string Parse(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return "دستگاه نامشخص";

        var operatingSystem = ParseOperatingSystem(userAgent);
        var browser = ParseBrowserName(userAgent);

        return operatingSystem == OperatingSystemType.Unknown
            ? browser
            : $"{browser} روی {GetOperatingSystemDisplayName(operatingSystem)}";
    }

    public static DeviceType ParseDeviceType(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return DeviceType.Unknown;

        var agent = userAgent.ToLowerInvariant();

        if (agent.Contains("ipad")
            || agent.Contains("tablet")
            || agent.Contains("playbook")
            || (agent.Contains("android") && !agent.Contains("mobile")))
            return DeviceType.Tablet;

        if (agent.Contains("iphone")
            || agent.Contains("ipod")
            || agent.Contains("windows phone")
            || agent.Contains("blackberry")
            || (agent.Contains("android") && agent.Contains("mobile"))
            || (agent.Contains("mobile") && !agent.Contains("ipad")))
            return DeviceType.Mobile;

        if (agent.Contains("windows")
            || agent.Contains("macintosh")
            || agent.Contains("mac os")
            || agent.Contains("linux")
            || agent.Contains("cros"))
            return DeviceType.Desktop;

        return DeviceType.Unknown;
    }

    public static OperatingSystemType ParseOperatingSystem(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return OperatingSystemType.Unknown;

        var agent = userAgent.ToLowerInvariant();

        if (agent.Contains("android"))
            return OperatingSystemType.Android;

        if (agent.Contains("iphone") || agent.Contains("ipad") || agent.Contains("ipod"))
            return OperatingSystemType.iOS;

        if (agent.Contains("cros"))
            return OperatingSystemType.ChromeOs;

        if (agent.Contains("windows"))
            return OperatingSystemType.Windows;

        if (agent.Contains("mac os") || agent.Contains("macintosh"))
            return OperatingSystemType.MacOs;

        if (agent.Contains("linux"))
            return OperatingSystemType.Linux;

        return OperatingSystemType.Unknown;
    }

    private static string ParseBrowserName(string userAgent)
    {
        var agent = userAgent.ToLowerInvariant();

        if (agent.Contains("edg/"))
            return "Edge";

        if (agent.Contains("chrome/") && !agent.Contains("edg/"))
            return "Chrome";

        if (agent.Contains("firefox/"))
            return "Firefox";

        if (agent.Contains("safari/") && !agent.Contains("chrome/"))
            return "Safari";

        if (agent.Contains("opr/") || agent.Contains("opera"))
            return "Opera";

        return "مرورگر";
    }

    private static string GetOperatingSystemDisplayName(OperatingSystemType operatingSystem)
        => operatingSystem switch
        {
            OperatingSystemType.Windows => "Windows",
            OperatingSystemType.MacOs => "macOS",
            OperatingSystemType.Linux => "Linux",
            OperatingSystemType.Android => "Android",
            OperatingSystemType.iOS => "iOS",
            OperatingSystemType.ChromeOs => "ChromeOS",
            _ => "سیستم‌عامل"
        };
}
