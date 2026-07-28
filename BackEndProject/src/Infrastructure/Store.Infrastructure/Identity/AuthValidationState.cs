using Edition.Application.Contracts;

namespace Store.Infrastructure.Identity;

public sealed class AuthValidationState : IAuthValidationState
{
    public bool? CachedResult { get; set; }
}
