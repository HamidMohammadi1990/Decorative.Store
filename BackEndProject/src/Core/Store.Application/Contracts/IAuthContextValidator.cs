using System.Security.Claims;

namespace Edition.Application.Contracts;

public interface IAuthContextValidator
{
    Task<bool> ValidateAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}