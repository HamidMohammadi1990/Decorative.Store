using Edition.Application.Contracts.Localization;

namespace Edition.Application.Services.Localization;

internal sealed class LanguageRegistryCacheData
{
    public long Version { get; init; }
    public List<LanguageInfo> Languages { get; init; } = [];
}