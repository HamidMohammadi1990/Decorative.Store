namespace Edition.Application.Contracts.Localization;

public interface ILanguageRegistry
{
    Task<IReadOnlyList<LanguageInfo>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LanguageInfo>> GetActiveLanguagesAsync(CancellationToken cancellationToken = default);
    Task<LanguageInfo?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<LanguageInfo?> GetByIdAsync(int languageId, CancellationToken cancellationToken = default);
    Task<LanguageInfo> GetDefaultAsync(CancellationToken cancellationToken = default);
    Task RefreshFromDatabaseAsync(CancellationToken cancellationToken = default);
    Task InvalidateAsync(CancellationToken cancellationToken = default);
}
