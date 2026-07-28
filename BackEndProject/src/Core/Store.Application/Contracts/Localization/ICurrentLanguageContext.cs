namespace Edition.Application.Contracts.Localization;

public interface ICurrentLanguageContext
{
    bool IsResolved { get; }
    int LanguageId { get; }
    string LanguageCode { get; }
}
