using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record SignUpUserRequest : IRequest<OperationResult<SignInUserResponse>>
{
    public string UserName { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
}
