namespace Edition.Application.Contracts.Localization;

public sealed record LanguageInfo(
    int Id,
    string Code,
    string Name,
    bool IsActive,
    bool IsDefault,
    int DisplayOrder,
    bool IsRtl);
