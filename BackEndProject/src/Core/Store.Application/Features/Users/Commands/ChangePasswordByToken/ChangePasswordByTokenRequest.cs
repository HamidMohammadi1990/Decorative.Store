using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Users.Commands;

public record ChangePasswordByTokenRequest : IRequest<OperationResult<ChangePasswordByTokenResponse>>
{
    public string Token { get; init; } = default!;
    public string UserName { get; init; } = default!;
    public string Password { get; init; } = default!;
    public ForgetPasswordOptionType OptionType { get; init; }
}