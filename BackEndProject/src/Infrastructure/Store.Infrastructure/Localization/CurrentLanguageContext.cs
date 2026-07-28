using Edition.Application.Contracts.Localization;

namespace Store.Infrastructure.Localization;

public sealed class CurrentLanguageContext : ICurrentLanguageContext, ICurrentLanguageContextInitializer
{
    private bool _isResolved;
    private int _languageId;
    private string _languageCode = string.Empty;

    public bool IsResolved => _isResolved;

    public int LanguageId =>
        _isResolved
            ? _languageId
            : throw new InvalidOperationException("Current language has not been resolved for this request.");

    public string LanguageCode =>
        _isResolved
            ? _languageCode
            : throw new InvalidOperationException("Current language has not been resolved for this request.");

    public void Initialize(int languageId, string languageCode)
    {
        _languageId = languageId;
        _languageCode = languageCode;
        _isResolved = true;
    }
}
