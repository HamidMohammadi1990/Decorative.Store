using Store.Common.Models;

namespace Edition.Application.Features.Users.Commands;

public record SignOutUserRequest(string token) : IRequest<OperationResult<bool>>
{
    public string Token { get; init; } = token;
}