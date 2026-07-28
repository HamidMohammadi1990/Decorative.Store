namespace Store.Common.Models;

public record ForwardedHeadersSettings
{
    public string[] KnownProxies { get; init; } = [];
}
