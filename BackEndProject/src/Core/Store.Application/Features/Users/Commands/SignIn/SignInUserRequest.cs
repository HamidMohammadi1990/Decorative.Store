using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record SignInUserRequest : IRequest<OperationResult<SignInUserResponse>>
{
    public string UserName { get; init; } = default!;
    public string Password { get; init; } = default!;
}