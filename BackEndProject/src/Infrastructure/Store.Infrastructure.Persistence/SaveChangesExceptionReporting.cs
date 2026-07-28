using Edition.Application.Contracts.Persistence;

namespace Store.Infrastructure.Persistence;

public sealed class SaveChangesExceptionReporting(bool includeTechnicalDetails) : ISaveChangesExceptionReporting
{
    public bool IncludeTechnicalDetails { get; } = includeTechnicalDetails;
}
