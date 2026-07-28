using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record ChangePasswordByOldPasswordRequest : IRequest<OperationResult<SignInUserResponse>>
{
    public string OldPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
}
