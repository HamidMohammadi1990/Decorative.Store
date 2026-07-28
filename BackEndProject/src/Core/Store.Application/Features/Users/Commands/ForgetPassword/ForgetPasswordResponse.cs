using Store.Domain.Enums;

namespace Edition.Application.Features.Users.Commands;

public record ForgetPasswordResponse
{
    public string Message { get; init; } = default!;
    public string UserName { get; init; } = default!;
    public ForgetPasswordOptionType OptionType { get; init; }
}