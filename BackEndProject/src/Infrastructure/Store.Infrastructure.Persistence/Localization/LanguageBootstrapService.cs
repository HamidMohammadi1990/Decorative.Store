using Edition.Application.Contracts.Localization;
using Store.Common.Localization;
using Store.Domain.Entities;
using Store.Domain.Repositories;

namespace Store.Infrastructure.Persistence.Localization;

public sealed class LanguageBootstrapService(
    EditionDbContext context,
    LocalizationSettings localizationSettings,
    ILanguageRepository languageRepository,
    ILanguageRegistry languageRegistry) : ILanguageBootstrapService
{
    public async Task EnsureLanguagesReadyAsync(CancellationToken cancellationToken = default)
    {
        if (!await languageRepository.AnyAsync(cancellationToken))
            await SeedLanguagesFromSettingsAsync(cancellationToken);

        await languageRegistry.RefreshFromDatabaseAsync(cancellationToken);
    }

    private async Task SeedLanguagesFromSettingsAsync(CancellationToken cancellationToken)
    {
        var cultures = localizationSettings.SupportedCultures
            .Select(LanguageCultureNormalizer.Normalize)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (cultures.Count == 0)
            cultures.Add(LanguageCultureNormalizer.Normalize(localizationSettings.DefaultCulture));

        var defaultCulture = LanguageCultureNormalizer.Normalize(localizationSettings.DefaultCulture);
        var languages = new List<Language>();

        for (var index = 0; index < cultures.Count; index++)
        {
            var code = cultures[index];
            languages.Add(Language.Create(
                code: code,
                name: GetDefaultName(code),
                isActive: true,
                isDefault: string.Equals(code, defaultCulture, StringComparison.OrdinalIgnoreCase),
                displayOrder: index + 1,
                isRtl: code.StartsWith("fa", StringComparison.OrdinalIgnoreCase)));
        }

        if (languages.All(x => !x.IsDefault) && languages.Count > 0)
        {
            var first = languages[0];
            languages[0] = Language.Create(
                first.Code,
                first.Name,
                first.IsActive,
                isDefault: true,
                first.DisplayOrder,
                first.IsRtl);
        }

        languageRepository.AddRange(languages);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static string GetDefaultName(string code)
        => code switch
        {
            "fa-IR" => "فارسی",
            "en-US" => "English",
            _ => code
        };
}
