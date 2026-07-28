using Edition.Application.Common.Caching.Abstractions;
using Edition.Application.Configurations.Localization;
using Edition.Application.Contracts.Localization;
using Microsoft.Extensions.DependencyInjection;
using Store.Common.Localization;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Edition.Application.Services.Localization;

public sealed class LanguageRegistry(
    IDistributedCache cache,
    ILanguageCacheConfiguration configuration,
    IServiceScopeFactory serviceScopeFactory,
    LocalizationSettings localizationSettings) : ILanguageRegistry
{
    public async Task<IReadOnlyList<LanguageInfo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var snapshot = await GetSnapshotAsync(cancellationToken);
        return snapshot.Languages;
    }

    public async Task<IReadOnlyList<LanguageInfo>> GetActiveLanguagesAsync(CancellationToken cancellationToken = default)
    {
        var snapshot = await GetSnapshotAsync(cancellationToken);
        return snapshot.Languages.Where(x => x.IsActive).ToList();
    }

    public async Task<LanguageInfo?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = LanguageCultureNormalizer.Normalize(code);
        var snapshot = await GetSnapshotAsync(cancellationToken);
        return snapshot.Languages.FirstOrDefault(x =>
            string.Equals(x.Code, normalizedCode, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<LanguageInfo?> GetByIdAsync(int languageId, CancellationToken cancellationToken = default)
    {
        var snapshot = await GetSnapshotAsync(cancellationToken);
        return snapshot.Languages.FirstOrDefault(x => x.Id == languageId);
    }

    public async Task<LanguageInfo> GetDefaultAsync(CancellationToken cancellationToken = default)
    {
        var snapshot = await GetSnapshotAsync(cancellationToken);
        var defaultLanguage = snapshot.Languages.FirstOrDefault(x => x.IsDefault && x.IsActive)
                              ?? snapshot.Languages.FirstOrDefault(x => x.IsDefault)
                              ?? snapshot.Languages.FirstOrDefault(x =>
                                  string.Equals(
                                      x.Code,
                                      LanguageCultureNormalizer.Normalize(localizationSettings.DefaultCulture),
                                      StringComparison.OrdinalIgnoreCase))
                              ?? snapshot.Languages.FirstOrDefault();

        if (defaultLanguage is null)
            throw new InvalidOperationException("No languages are configured in the language registry.");

        return defaultLanguage;
    }

    public async Task RefreshFromDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<ILanguageRepository>();
        var languages = await repository.GetAllOrderedAsync(cancellationToken);

        var version = await GetCurrentVersionAsync(cancellationToken) + 1;
        var payload = new LanguageRegistryCacheData
        {
            Version = version,
            Languages = languages.Select(Map).ToList()
        };

        await cache.SetAsync(
            configuration.DataKey,
            payload,
            configuration.CacheTtl,
            configuration.CacheInstance,
            token: cancellationToken);

        await cache.SetAsync(
            configuration.VersionKey,
            version,
            configuration.CacheTtl,
            configuration.CacheInstance,
            token: cancellationToken);
    }

    public async Task InvalidateAsync(CancellationToken cancellationToken = default)
    {
        var version = await GetCurrentVersionAsync(cancellationToken) + 1;

        await cache.SetAsync(
            configuration.VersionKey,
            version,
            configuration.CacheTtl,
            configuration.CacheInstance,
            token: cancellationToken);

        await cache.RemoveAsync(configuration.DataKey, configuration.CacheInstance, cancellationToken);
    }

    private async Task<LanguageRegistryCacheData> GetSnapshotAsync(CancellationToken cancellationToken)
    {
        var cached = await cache.GetAsync<LanguageRegistryCacheData>(
            configuration.DataKey,
            configuration.CacheInstance,
            cancellationToken);

        if (cached is { Languages.Count: > 0 })
            return cached;

        await RefreshFromDatabaseAsync(cancellationToken);

        cached = await cache.GetAsync<LanguageRegistryCacheData>(
            configuration.DataKey,
            configuration.CacheInstance,
            cancellationToken);

        if (cached is { Languages.Count: > 0 })
            return cached;

        throw new InvalidOperationException("Language registry cache could not be loaded.");
    }

    private async Task<long> GetCurrentVersionAsync(CancellationToken cancellationToken)
    {
        var version = await cache.GetAsync<long>(
            configuration.VersionKey,
            configuration.CacheInstance,
            cancellationToken);

        return version;
    }

    private static LanguageInfo Map(Language language)
        => new(
            language.Id,
            language.Code,
            language.Name,
            language.IsActive,
            language.IsDefault,
            language.DisplayOrder,
            language.IsRtl);
}
