namespace Store.Common.Localization;

public record LocalizationSettings
{
    public string DefaultCulture { get; init; } = "fa-IR";
    public string DefaultUICulture { get; init; } = "fa-IR";
    public string[] SupportedCultures { get; init; } = ["fa-IR", "en-US"];
    public string[] SupportedUICultures { get; init; } = ["fa-IR", "en-US"];
}
