namespace Store.Common.Models;

public record ForwardedHeadersSettings
{
    public string[] KnownProxies { get; init; } = [];

    /// <summary>When true, trust X-Forwarded-* from the reverse proxy (e.g. Docker nginx). Use only when the API is not exposed directly.</summary>
    public bool TrustReverseProxy { get; init; }
}
