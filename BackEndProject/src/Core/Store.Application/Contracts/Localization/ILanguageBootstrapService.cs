namespace Edition.Application.Contracts.Localization;

public interface ILanguageBootstrapService
{
    Task EnsureLanguagesReadyAsync(CancellationToken cancellationToken = default);
}
