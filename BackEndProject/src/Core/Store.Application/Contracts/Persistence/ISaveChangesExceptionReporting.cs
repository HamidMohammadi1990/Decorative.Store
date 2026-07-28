namespace Edition.Application.Contracts.Persistence;

/// <summary>
/// Controls whether raw database exception details are returned to callers (typically enabled in Development).
/// </summary>
public interface ISaveChangesExceptionReporting
{
    bool IncludeTechnicalDetails { get; }
}
