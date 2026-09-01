using Store.Domain.Common;

namespace Store.Domain.Entities;

public class MarketingStripDisclaimer : BaseEntity
{
    public int LanguageId { get; private set; }
    public string? Disclaimer { get; private set; }
    public string? DisclaimerLinkLabel { get; private set; }
    public string? DisclaimerLinkHref { get; private set; }

    public Language Language { get; private set; } = default!;

    public static MarketingStripDisclaimer Create(
        int languageId,
        string? disclaimer,
        string? disclaimerLinkLabel,
        string? disclaimerLinkHref)
        => new()
        {
            LanguageId = languageId,
            Disclaimer = disclaimer,
            DisclaimerLinkLabel = disclaimerLinkLabel,
            DisclaimerLinkHref = disclaimerLinkHref,
        };

    public void Update(string? disclaimer, string? disclaimerLinkLabel, string? disclaimerLinkHref)
    {
        Disclaimer = disclaimer;
        DisclaimerLinkLabel = disclaimerLinkLabel;
        DisclaimerLinkHref = disclaimerLinkHref;
    }
}
