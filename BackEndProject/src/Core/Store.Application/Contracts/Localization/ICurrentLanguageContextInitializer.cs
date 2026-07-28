namespace Edition.Application.Contracts.Localization;

public interface ICurrentLanguageContextInitializer
{
    void Initialize(int languageId, string languageCode);
}
