using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record SignInUserByPhoneNumberRequest : IRequest<OperationResult<SignInUserResponse>>
{
    public string Token { get; init; } = default!;
    public string UserName { get; init; } = default!;
}