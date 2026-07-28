namespace Edition.Application.Contracts.Infrastructure;

public interface IEmailService
{
    bool Send(string to, string message, string title);
    Task<string> GenerateTokenAsync(string phoneNumber);
    Task<bool> VerifyTokenAsync(string token, string email);
}